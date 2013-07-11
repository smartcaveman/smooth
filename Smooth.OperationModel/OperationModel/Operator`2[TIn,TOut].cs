using System.Diagnostics.Contracts;

namespace Smooth.OperationModel
{
    using System;
    using Operands;

    public abstract class Operator<TIn, TOut> : IOperator<TIn, TOut>
        where TIn : ISource
    {
        private readonly string symbol;

        public Operator(string symbol)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(symbol));
            this.symbol = symbol;
        }

        public string Symbol
        {
            get { return this.symbol; }
        }

        IOperationResult<TOut> IOperator<TOut>.Process(ISource input)
        {
            return Process(input);
        }

        IOperationResult IOperator.Process(ISource input)
        {
            return Process(input);
        }

        public abstract IOperationResult<TOut> Process(TIn input);

        protected abstract IOperationResult<TOut> Process(ISource input);
    }
}