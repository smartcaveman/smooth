using System;
using System.Diagnostics.Contracts;
using Smooth.Operands;

namespace Smooth.OperationModel
{
    [ContractClass(typeof(IOperatorContract<>))]
    public interface IOperator<out TOut> : IOperator
    {
        [Pure]
        new IOperationResult<TOut> Process(ISource input);
    }

    [Pure, ContractClassFor(typeof(IOperator<>))]
    internal abstract class IOperatorContract<TOut> : IOperator<TOut>
    {
        [Pure]
        IOperationResult<TOut> IOperator<TOut>.Process(ISource input)
        {
            Contract.Requires<ArgumentNullException>(!ReferenceEquals(input, null));
            Contract.Ensures(Contract.Result<IOperationResult<TOut>>() != null);
            return default(IOperationResult<TOut>);
        }

        string IOperator.Symbol
        {
            get { throw new NotImplementedException(); }
        }

        IOperationResult IOperator.Process(ISource input)
        {
            throw new NotImplementedException();
        }
    }
}