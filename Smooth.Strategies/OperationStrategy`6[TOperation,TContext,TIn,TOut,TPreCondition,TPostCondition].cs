using System;
using Smooth.Operands;
using Smooth.OperationModel;

namespace Smooth.Strategies
{
    public abstract class OperationStrategy<TOperation, TContext, TIn, TOut, TPreCondition, TPostCondition> :
        OperationStrategy<TOperation, TContext, TOut, TPreCondition, TPostCondition>
        where TOperation : class /*delegate*/
        where TPreCondition : class /*delegate*/
        where TPostCondition : class /*delegate*/
        where TIn : ISource
    {
        protected OperationStrategy(
            DelegateRewriter delegateRewriter,
            TOperation operation,
            TPreCondition preCondition = null,
            TPostCondition postCondition = null,
            Predicate<TContext> invariant = null)
            : base(delegateRewriter, operation, preCondition, postCondition, invariant)
        {
        }

        protected OperationStrategy(DelegateRewriter delegateRewriter,
                                          Func<TOperation> loadOperation, 
            TPreCondition preCondition = null,
                                          TPostCondition postCondition = null,
                                          Predicate<TContext> invariant = null)
            : base(delegateRewriter, loadOperation, preCondition, postCondition, invariant)
        {
        }

        public TOut this[TIn input]
        {
            get { return ApplyOperation(input); }
        }

        public IResult<TOut> Apply(TIn input)
        {
            return Apply(new object[] { input });
        }

        private TOut ApplyOperation(TIn input)
        {
            IResult<TOut> operationResult = Apply(input);
            if (operationResult.IsError)
            {
                throw operationResult.IsException ? operationResult.Exception : new InvalidOperationException();
            }
            return operationResult.Output;
        }

        protected abstract TOperation Operation(Func<TIn, TOut> func);

        public override sealed TOperation ToOperation()
        {
            return Operation(ApplyOperation);
        }
    }
}