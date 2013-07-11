using System;
using System.Diagnostics.Contracts;
using Smooth.Operands;

namespace Smooth.OperationModel
{
    [ContractClass(typeof(IOperatorContract))]
    public interface IOperator
    {
        [Pure]
        string Symbol { get; }

        [Pure]
        IOperationResult Process(ISource input);
    }

    [Pure, ContractClassFor(typeof(IOperator))]
    internal abstract class IOperatorContract : IOperator
    {
        [Pure]
        string IOperator.Symbol
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
                return default(string);
            }
        }

        [Pure]
        IOperationResult IOperator.Process(ISource input)
        {
            Contract.Requires<ArgumentNullException>(!ReferenceEquals(input, null));
            Contract.Ensures(Contract.Result<IOperationResult>() != null);
            return default(IOperationResult);
        }
    }
}