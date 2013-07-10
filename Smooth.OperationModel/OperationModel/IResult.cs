using System;
using Smooth.Operands;

namespace Smooth.OperationModel
{
    public interface IResult
    {
        bool IsAbandoned { get; }

        bool IsCompleted { get; }

        bool IsError { get; }

        bool IsException { get; }

        bool IsValid { get; }

        bool IsPostValidationError { get; }

        bool IsPreValidationError { get; }

        bool IsValidationError { get; }

        Exception Exception { get; }

        object Output { get; }

        DateTimeOffset TimeStamp { get; }

        IUnarySource ToSource();
    }
}