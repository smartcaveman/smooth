using System;
using System.Diagnostics.Contracts;
using Smooth.ProcessModel;

namespace Smooth.Strategies
{
    public class ProcessStrategy<TContext> : Strategy<Action<TContext>>, IProcessStrategy<TContext>
    {
        private static readonly Predicate<TContext> Any;
        private Action<TContext> action;
        private readonly Func<Action<TContext>> loadAction;
        private readonly Predicate<TContext> preCondition, postCondition, invariant;

        static ProcessStrategy()
        {
            Any = x => true;
        }

        public ProcessStrategy(
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

        public ProcessStrategy(
            Action<TContext> action,
            Predicate<TContext> preCondition = null,
            Predicate<TContext> postCondition = null,
            Predicate<TContext> invariant = null)
            : this(preCondition, postCondition, invariant)
        {
            Contract.Requires<ArgumentNullException>(!ReferenceEquals(action, null));
            this.action = action;
        }

        private ProcessStrategy(
            Predicate<TContext> preCondition = null,
            Predicate<TContext> postCondition = null,
            Predicate<TContext> invariant = null)
        {
            this.preCondition = preCondition;
            this.postCondition = postCondition;
            this.invariant = invariant;
        }

        protected IProcessStrategy<TContext> Action
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

        ProcessResult IProcessStrategy<TContext>.Execute(TContext context)
        {
            bool preValidate, postValidate;
            try
            {
                preValidate = PreCondition.Invoke(context) && Invariant.Invoke(context);
            }
            catch (Exception exception)
            {
                return ProcessResult.PreConditionException(exception);
            }
            if (!preValidate)
            {
                return ProcessResult.PreConditionFailed();
            }
            try
            {
                OriginalDelegate.Invoke(context);
            }
            catch (Exception exception)
            {
                return ProcessResult.ProcessException(exception);
            }
            try
            {
                postValidate = PostCondition.Invoke(context) && Invariant.Invoke(context);
            }
            catch (Exception exception)
            {
                return ProcessResult.PostConditionException(exception);
            }
            if (!postValidate)
            {
                return ProcessResult.PostConditionFailed();
            }
            return ProcessResult.Completed();
        }

        private void Execute(TContext context)
        {
            ProcessResult result = Action.Execute(context);
            if (result.HasError)
            {
                throw result.HasException ? result.Exception : new InvalidOperationException();
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