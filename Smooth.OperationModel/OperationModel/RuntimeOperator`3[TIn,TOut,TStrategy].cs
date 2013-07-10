using System;
using System.Diagnostics.Contracts;
using Smooth.Operands;
using Smooth.Strategies;

namespace Smooth.OperationModel
{
    public class RuntimeOperator<TIn, TOut, TStrategy> : Operator<TIn, TOut>
        where TIn : ISource
        where TStrategy : IOperationStrategy<Func<TIn, TOut>, Predicate<TIn>, Predicate<TOut>, TOut>
    {
        private readonly TStrategy runtimeStrategy;

        public RuntimeOperator(string symbol, TStrategy runtimeStrategy)
            : base(symbol)
        {
            Contract.Requires<ArgumentNullException>(!ReferenceEquals(runtimeStrategy, null));
            this.runtimeStrategy = runtimeStrategy;
        }

        public TStrategy RuntimeStrategy
        {
            get
            {
                Contract.Ensures(!ReferenceEquals(null, Contract.Result<TStrategy>()));
                return this.runtimeStrategy;
            }
        }

        protected override IResult<TOut> Operate(ISource input)
        {
            return RuntimeStrategy.Apply(new object[] {input});
        }
    }
}