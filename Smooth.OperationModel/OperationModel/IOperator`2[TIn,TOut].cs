﻿using System;
using System.Diagnostics.Contracts;

namespace Smooth.OperationModel
{
    using Operands;

    [ContractClass(typeof(IOperatorContract<,>))]
    public interface IOperator<in TIn, out TOut> : IOperator<TOut>
        where TIn : ISource
    {
        [Pure]
        IOperationResult<TOut> Process(TIn input);
    }

    [Pure, ContractClassFor(typeof(IOperator<,>))]
    internal abstract class IOperatorContract<TIn, TOut> : IOperator<TIn, TOut>
        where TIn : ISource
    {
        [Pure]
        public IOperationResult<TOut> Process(TIn input)
        {
            Contract.Requires<ArgumentNullException>(!ReferenceEquals(input, null));
            Contract.Ensures(Contract.Result<IOperationResult<TOut>>() != null);
            return default(IOperationResult<TOut>);
        }

        string IOperator.Symbol
        {
            get { throw new NotImplementedException(); }
        }

        IOperationResult<TOut> IOperator<TOut>.Process(ISource input)
        {
            throw new NotImplementedException();
        }

        IOperationResult IOperator.Process(ISource input)
        {
            throw new NotImplementedException();
        }
    }
}