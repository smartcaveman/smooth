using System;
using System.Diagnostics.Contracts;
using System.Linq;
using Smooth.Operands;
using Smooth.Strategies;

namespace Smooth.OperationModel
{
    public class RuntimeOperator<TIn, TOut, TStrategy> : Operator<TIn, TOut>
        where TIn : ISource
        where TStrategy : IOperationStrategy<Func<TIn, TOut>, Predicate<TIn>, Predicate<TOut>, TIn, TOut>
    {
        private readonly TStrategy runtimeStrategy;

        public RuntimeOperator(string symbol, Func<IOperator<TIn, TOut>, TStrategy> runtimeStrategyFactory)
            : base(symbol)
        {
            Contract.Requires<ArgumentNullException>(!ReferenceEquals(runtimeStrategyFactory, null));
            this.runtimeStrategy = runtimeStrategyFactory(this);
        }

        public TStrategy RuntimeStrategy
        {
            get
            {
                Contract.Ensures(!ReferenceEquals(null, Contract.Result<TStrategy>()));
                return this.runtimeStrategy;
            }
        }

        public override IOperationResult<TOut> Process(TIn input)
        {
            return RuntimeStrategy.Apply(input);
        }

        protected override IOperationResult<TOut> Process(ISource input)
        {
            return input is TIn ? RuntimeStrategy.Apply((TIn)input) : RuntimeStrategy.Apply(input.ToNary().Operands.Cast<object>().ToArray());
        }
    }
}