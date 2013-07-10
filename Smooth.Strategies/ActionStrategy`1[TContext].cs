using System;
using System.Diagnostics.Contracts;
using Smooth.OperationModel;

namespace Smooth.Strategies
{
    public class ActionStrategy<TContext> : Strategy<Action<TContext>>, IActionStrategy<TContext>
    {
        private static readonly Predicate<TContext> Any;
        private Action<TContext> action;
        private readonly Func<Action<TContext>> loadAction;
        private readonly Predicate<TContext> preCondition, postCondition, invariant;

        static ActionStrategy()
        {
            Any = x => true;
        }

        public ActionStrategy(
            Func<Action<TContext>> loadAction,
            Predicate<TContext> preCondition = null,
            Predicate<TContext> postCondition = null,
            Predicate<TContext> invariant = null)
            : this(preCondition, postCondition, invariant)
        {
            Contract.Requires<ArgumentNullException>(!ReferenceEquals(loadAction, null));
            this.loadAction = loadAction;
            this.preCondition = preCondition;
            this.postCondition = postCondition;
            this.invariant = invariant;
        }

        public ActionStrategy(
            Action<TContext> action,
            Predicate<TContext> preCondition = null,
            Predicate<TContext> postCondition = null,
            Predicate<TContext> invariant = null)
            :this(preCondition,postCondition,invariant)
        {
            Contract.Requires<ArgumentNullException>(!ReferenceEquals(action, null));
            this.action = action; 
        }

        private ActionStrategy(
            Predicate<TContext> preCondition = null,
            Predicate<TContext> postCondition = null,
            Predicate<TContext> invariant = null)
        {
            this.preCondition = preCondition;
            this.postCondition = postCondition;
            this.invariant = invariant;
        }

        protected IActionStrategy<TContext> Action
        {
            get { return this; }
        }

        protected virtual Predicate<TContext> PreCondition
        {
            get { return preCondition ?? Any; }
        }

        protected virtual Predicate<TContext> PostCondition
        {
            get { return postCondition ?? Any; }
        }

        protected virtual Predicate<TContext> Invariant
        {
            get { return invariant ?? Any; }
        }

        Predicate<TContext> IContractGraph<Predicate<TContext>>.Invariant
        {
            get { return Invariant ?? Any; }
        }

        Predicate<TContext> IContractGraph<Predicate<TContext>>.PreCondition
        {
            get { return PreCondition ?? Any; }
        }

        Predicate<TContext> IContractGraph<Predicate<TContext>>.PostCondition
        {
            get { return PostCondition ?? Any; }
        }

        IResult<TContext> IActionStrategy<TContext>.Execute(TContext context)
        {
            bool preValidate, postValidate;
            try
            {
                preValidate = PreCondition.Invoke(context) && Invariant.Invoke(context);
            }
            catch (Exception exception)
            {
                return OperationResult<TContext>.PreConditionException(exception);
            }
            if (!preValidate)
            {
                return OperationResult<TContext>.PreConditionFailed();
            }
            try
            {
                OriginalDelegate.Invoke(context);
            }
            catch (Exception exception)
            {
                return OperationResult<TContext>.OperationException(exception);
            }
            try
            {
                postValidate = PostCondition.Invoke(context) && Invariant.Invoke(context);
            }
            catch (Exception exception)
            {
                return OperationResult<TContext>.PostConditionException(context, exception);
            }
            if (!postValidate)
            {
                return OperationResult<TContext>.PostConditionFailed(context);
            }
            return OperationResult<TContext>.Completed(context);
        }
         
        private void Execute(TContext context)
        {
            IResult<TContext> operationResult = Action.Execute(context);
            if (operationResult.IsError)
            {
                throw operationResult.IsException ? operationResult.Exception : new InvalidOperationException();
            }
        }

        protected sealed override Action<TContext> InterceptedDelegate
        {
            get { return Execute; }
        }

        protected sealed override Action<TContext> ResolveOriginalDelegate()
        {
            return action ?? (action = loadAction());
        }
    }
}