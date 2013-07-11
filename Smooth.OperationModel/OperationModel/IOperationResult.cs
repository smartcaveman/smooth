using System;
using System.Diagnostics.Contracts;
using Smooth.Operands;
using Smooth.ProcessModel;

namespace Smooth.OperationModel
{
    [ContractClass(typeof(IOperationResultContract))]
    public interface IOperationResult : IResult
    {
        IOperator Operator { get; }

        object Output { get; }

        IUnarySource ToSource();
    }

    [ContractClassFor(typeof(IOperationResult))]
    internal abstract class IOperationResultContract : IOperationResult
    {
        IOperator IOperationResult.Operator
        {
            get
            {
                Contract.Ensures(Contract.Result<IOperator>() != null);
                return default(IOperator);
            }
        }

        object IOperationResult.Output
        {
            get { throw new NotImplementedException(); }
        }

        IUnarySource IOperationResult.ToSource()
        {
            Contract.Ensures(Contract.Result<IUnarySource>() != null && Contract.Result<IUnarySource>().Operand.Equals(((IOperationResult)this).Output));
            return default(IUnarySource);
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
    }
}