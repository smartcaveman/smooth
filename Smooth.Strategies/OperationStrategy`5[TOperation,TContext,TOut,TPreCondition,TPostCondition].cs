using System;
using Smooth.OperationModel;

namespace Smooth.Strategies
{
    public abstract class OperationStrategy<TOperation, TContext, TOutput, TPreCondition, TPostCondition>
        : ActionStrategy<TContext>, IOperationStrategy<TOperation, TPreCondition, TPostCondition, TOutput>
        where TOperation : class /*delegate*/
        where TPreCondition : class /*delegate*/
        where TPostCondition : class /*delegate*/
    {
        private readonly TPostCondition postCondition;
        private readonly TPreCondition preCondition;

        protected OperationStrategy(
            DelegateRewriter delegateRewriter,
            TOperation operation,
            TPreCondition preCondition = null,
            TPostCondition postCondition = null,
            Predicate<TContext> invariant = null)
            : base(
                action: delegateRewriter.RewriteAction(operation),
                preCondition: delegateRewriter.RewritePreCondition(preCondition),
                postCondition: delegateRewriter.RewritePostCondition(postCondition),
                invariant: invariant)
        {
            this.preCondition = preCondition;
            this.postCondition = postCondition;
        }

        protected OperationStrategy(
            DelegateRewriter delegateRewriter,
            Func<TOperation> loadOperation,
            TPreCondition preCondition = null,
            TPostCondition postCondition = null,
            Predicate<TContext> invariant = null)
            : base(
                loadAction: () => delegateRewriter.RewriteAction(loadOperation()),
                preCondition: delegateRewriter.RewritePreCondition(preCondition),
                postCondition: delegateRewriter.RewritePostCondition(postCondition),
                invariant: invariant)
        {
            this.preCondition = preCondition;
            this.postCondition = postCondition;
        }

        protected OperationStrategy(Func<Action<TContext>> getTheAction,
                                          Predicate<TContext> invariant = null,
                                          Predicate<TContext> preCondition = null,
                                          Predicate<TContext> postCondition = null)
            : base(getTheAction, invariant, preCondition, postCondition)
        {
        }

        protected OperationStrategy(
            Action<TContext> theAction,
            Predicate<TContext> invariant = null,
            Predicate<TContext> preCondition = null,
            Predicate<TContext> postCondition = null)
            : base(theAction, invariant, preCondition, postCondition)
        {
        }

        public abstract TOperation ToOperation();

        TPreCondition IOperationStrategy<TOperation, TPreCondition, TPostCondition, TOutput>.PreCondition
        {
            get { return preCondition; }
        }

        TPostCondition IOperationStrategy<TOperation, TPreCondition, TPostCondition, TOutput>.PostCondition
        {
            get { return postCondition; }
        }

        IResult<TOutput> IOperationStrategy<TOperation, TPreCondition, TPostCondition, TOutput>.Apply(
            object[] input)
        {
            return Apply(input);
        }

        protected IResult<TOutput> Apply(object[] input)
        {
            return Action.Execute(Context(input)).Select(Output);
        }

        protected abstract TContext Context(object[] input);

        protected abstract TOutput Output(TContext context);

        protected abstract class DelegateRewriter
        {
            public abstract Action<TContext> RewriteAction(TOperation operation);

            public abstract Predicate<TContext> RewritePreCondition(TPreCondition preCondition);

            public abstract Predicate<TContext> RewritePostCondition(TPostCondition postCondition);
        }

        TOperation IStrategy<TOperation>.Delegate
        {
            get { return ToOperation(); }
        }
    }
}