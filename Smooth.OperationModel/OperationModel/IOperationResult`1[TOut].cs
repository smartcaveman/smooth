using System;
using System.Diagnostics.Contracts;
using Smooth.Operands;
using Smooth.ProcessModel;

namespace Smooth.OperationModel
{
    public interface IOperationResult<out TOut> : IOperationResult
    {
        new IOperator<TOut> Operator { get; }

        new TOut Output { get; }

        IOperationResult<TNext> Select<TNext>(Func<TOut, TNext> f);

        new IUnarySource<TOut> ToSource();
    }

    internal abstract class IOperationResultContract<T> : IOperationResult<T>
    {
        IOperator<T> IOperationResult<T>.Operator
        {
            get
            {
                Contract.Ensures(Contract.Result<IOperator<T>>() != null);
                return default(IOperator<T>);
            }
        }
         

        bool IResult.HasError
        {
            get { throw new NotImplementedException(); }
        }

        bool IResult.HasException
        {
            get { throw new NotImplementedException(); }
        }

        bool IResult.HasPostValidationError
        {
            get { throw new NotImplementedException(); }
        }

        bool IResult.HasPreValidationError
        {
            get { throw new NotImplementedException(); }
        }

        bool IResult.HasValidationError
        {
            get { throw new NotImplementedException(); }
        }

        Exception IResult.Exception
        {
            get { throw new NotImplementedException(); }
        }

        DateTimeOffset IResult.TimeStamp
        {
            get { throw new NotImplementedException(); }
        }

        IOperator IOperationResult.Operator
        {
            get { throw new NotImplementedException(); }
        }

        object IOperationResult.Output
        {
            get { throw new NotImplementedException(); }
        }

        T IOperationResult<T>.Output
        {
            get { throw new NotImplementedException(); }
        }

        IOperationResult<TNext> IOperationResult<T>.Select<TNext>(Func<T, TNext> f)
        {
            Contract.Requires<ArgumentNullException>(f != null);
            Contract.Ensures(Contract.Result<IOperationResult<TNext>>() != null);
            return default(IOperationResult<TNext>);
        }

        IUnarySource<T> IOperationResult<T>.ToSource()
        {
            Contract.Ensures(Contract.Result<IUnarySource<T>>() != null && Contract.Result<IUnarySource<T>>().Operand.Equals(((IOperationResult<T>)this).Output));
            return default(IUnarySource<T>);
        }

        IUnarySource IOperationResult.ToSource()
        {
            throw new NotImplementedException();
        }
    }
}