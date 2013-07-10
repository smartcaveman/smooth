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
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(symbol));
            this.symbol = symbol;
        }

        public string Symbol
        {
            get { return this.symbol; }
        }

        IResult IOperator.Operate(ISource input)
        {
            return Operate(input);
        }

        IResult<TOut> IOperator<TIn, TOut>.Operate(TIn input)
        {
            return Operate(input);
        }

        protected abstract IResult<TOut> Operate(ISource input);
    }
}