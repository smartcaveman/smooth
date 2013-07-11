using System;
using Smooth.Operands;
using Smooth.OperationModel;

namespace Smooth.Strategies
{
    public abstract class OperationStrategy<TOperation, TContext, TIn, TOut, TPreCondition, TPostCondition> :
        ProcessStrategy<TContext>, IOperator<TIn, TOut>,
        IOperationStrategy<TOperation, TPreCondition, TPostCondition, TIn, TOut>, 
        IOperationStrategy<TOperation, TPreCondition, TPostCondition, TOut> where TOperation : class /*delegate*/
        where TPreCondition : class /*delegate*/
        where TPostCondition : class /*delegate*/
        where TIn : ISource
    {
        private readonly IOperator<TIn, TOut> @operator;
        private TPostCondition postCondition;
        private TPreCondition preCondition;

        protected OperationStrategy(
            IOperator<TIn, TOut> @operator,
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
            this.@operator = @operator;
            this.preCondition = preCondition;
            this.postCondition = postCondition;
        }

        protected OperationStrategy(
            IOperator<TIn, TOut> @operator, DelegateRewriter delegateRewriter,
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
            this.@operator = @operator;
            this.preCondition = preCondition;
            this.postCondition = postCondition;
        }

        public TOut this[TIn input]
        {
            get { return ApplyOperation(input); }
        }

        public abstract IOperationResult<TOut> Apply(TIn input);

        protected IOperationResult<TOut> Apply(object[] input)
        {
            return Apply(Input(input)); 
        }

        protected abstract TIn Input(object[] input);

        protected abstract TContext Context(TIn input);

        protected abstract TOut Output(TContext context);

        private TOut ApplyOperation(TIn input)
        {
            IOperationResult<TOut> operationResult = Apply(input);
            if (operationResult.HasError)
            {
                throw operationResult.HasException ? operationResult.Exception : new InvalidOperationException();
            }
            return operationResult.Output;
        }

        protected abstract TOperation Operation(Func<TIn, TOut> func);

        public TOperation ToOperation()
        {
            return Operation(ApplyOperation);
        }

        IOperationResult<TOut> IOperator<TIn, TOut>.Process(TIn input)
        {
            return @operator.Process(input);
        }

        string IOperator.Symbol
        {
            get { return @operator.Symbol; }
        }

        TPreCondition IOperationStrategy<TOperation, TPreCondition, TPostCondition, TOut>.PreCondition
        {
            get { return preCondition; }
        }

        TPostCondition IOperationStrategy<TOperation, TPreCondition, TPostCondition, TOut>.PostCondition
        {
            get { return postCondition; }
        }

        TOperation IStrategy<TOperation>.Delegate
        {
            get { return ToOperation(); }
        }

        IOperationResult<TOut> IOperator<TOut>.Process(ISource input)
        {
            return @operator.Process(input);
        }

        IOperationResult IOperator.Process(ISource input)
        {
            return @operator.Process(input);
        }

        IOperationResult<TOut> IOperationStrategy<TOperation, TPreCondition, TPostCondition, TOut>.Apply(
            object[] input)
        {
            return Apply(input);
        }

        protected abstract class DelegateRewriter
        {
            public abstract Action<TContext> RewriteAction(TOperation operation);

            public abstract Predicate<TContext> RewritePreCondition(TPreCondition preCondition);

            public abstract Predicate<TContext> RewritePostCondition(TPostCondition postCondition);
        }
    }
}