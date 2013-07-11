using System;
using System.Diagnostics.Contracts;

namespace Smooth.ProcessModel
{
    [ContractClass(typeof(IResultContract))]
    public interface IResult
    { 
        bool HasError { get; }

        bool HasValidationError { get; }

        bool HasPreValidationError { get; }

        bool HasPostValidationError { get; }

        bool HasException { get; }

        Exception Exception { get; }

        DateTimeOffset TimeStamp { get; }
    }

    [ContractClassFor(typeof(IResult))]
    internal abstract class IResultContract : IResult
    {

        bool IResult.HasError
        {
            get { throw new NotImplementedException(); }
        }

        bool IResult.HasException
        {
            get
            {
                Contract.Ensures(!Contract.Result<bool>() || ((IResult)this).HasError, "Exception -> Error");
                return default(bool);
            }
        }

        bool IResult.HasPostValidationError
        {
            get
            {
                Contract.Ensures(!Contract.Result<bool>() || ((IResult)this).HasError, "PostValidationError -> Error");
                return default(bool);
            }
        }

        bool IResult.HasPreValidationError
        {
            get
            {
                Contract.Ensures(!Contract.Result<bool>() || ((IResult)this).HasError, "PreValidationError -> Error");
                return default(bool);
            }
        }

        bool IResult.HasValidationError
        {
            get
            {
                Contract.Ensures(Contract.Result<bool>() == (((IResult)this).HasPreValidationError || ((IResult)this).HasPostValidationError), "PreValidationError | PostValidationError <-> ValidationError");
                return default(bool);
            }
        }

        Exception IResult.Exception
        {
            get
            {
                Contract.Ensures(Contract.Result<Exception>() == null || ((IResult)this).HasException);
                return default(Exception);
            }
        }

        DateTimeOffset IResult.TimeStamp
        {
            get { throw new NotImplementedException(); }
        }
    }
}