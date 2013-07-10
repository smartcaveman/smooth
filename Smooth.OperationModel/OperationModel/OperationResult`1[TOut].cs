using System;
using Smooth.Operands;

namespace Smooth.OperationModel
{
    public class OperationResult<TOut> : IResult<TOut>
    {
        private readonly Exception exception;
        private readonly TOut operationOutput;
        private readonly DateTimeOffset timeStamp;
        private bool? postConditionOutput;
        private bool? preConditionOutput;

        private OperationResult(bool? preConditionOutput = default(bool?), TOut operationOutput = default(TOut),
                       bool? postConditionOutput = default(bool?), Exception exception = default(Exception))
        {
            this.preConditionOutput = preConditionOutput;
            this.operationOutput = operationOutput;
            this.postConditionOutput = postConditionOutput;
            this.exception = exception;
            timeStamp = DateTimeOffset.UtcNow;
        }

        public bool IsAbandoned
        {
            get { return !IsCompleted; }
        }

        public bool IsCompleted
        {
            get { return postConditionOutput.HasValue && postConditionOutput.Value; }
        }

        public bool IsError
        {
            get { return !IsValid; }
        }

        public bool IsException
        {
            get { return !ReferenceEquals(Exception, null); }
        }

        public bool IsValid
        {
            get { return postConditionOutput.HasValue && postConditionOutput.Value; }
        }

        public bool IsPostValidationError
        {
            get { return postConditionOutput.HasValue && !postConditionOutput.Value; }
        }

        public bool IsPreValidationError
        {
            get { return preConditionOutput.HasValue && !preConditionOutput.Value; }
        }

        public bool IsValidationError
        {
            get { return IsPreValidationError || IsPostValidationError; }
        }

        public Exception Exception
        {
            get { return exception; }
        }

        object IResult.Output
        {
            get { return Output; }
        }

        internal RelativeTime ExitPoint
        {
            get
            {
                return postConditionOutput.HasValue
                           ? RelativeTime.After
                           : (preConditionOutput.HasValue
                                  ? RelativeTime.During
                                  : RelativeTime.Before);
            }
        }

        public TOut Output
        {
            get { return operationOutput; }
        }

        public DateTimeOffset TimeStamp
        {
            get { return timeStamp; }
        }

        public static IResult<TOut> PreConditionFailed()
        {
            return new OperationResult<TOut>(preConditionOutput: false);
        }

        public static IResult<TOut> PreConditionException(Exception exception)
        {
            return new OperationResult<TOut>(exception: exception);
        }

        public static IResult<TOut> OperationException(Exception exception)
        {
            return new OperationResult<TOut>(preConditionOutput: true, exception: exception);
        }

        public static IResult<TOut> Completed(TOut output)
        {
            return new OperationResult<TOut>(true, output, true);
        }

        public static IResult<TOut> PostConditionFailed(TOut output)
        {
            return new OperationResult<TOut>(preConditionOutput: true, operationOutput: output, postConditionOutput: false);
        }

        public static IResult<TOut> PostConditionException(TOut output, Exception exception)
        {
            return new OperationResult<TOut>(preConditionOutput: true, operationOutput: output, exception: exception);
        }

        public IResult<SelectionType> Select<SelectionType>(Func<TOut, SelectionType> selector)
        {
            return new OperationResult<SelectionType>(preConditionOutput,
                                             ReferenceEquals(Output, null) ? default(SelectionType) : selector(Output),
                                             postConditionOutput, Exception);
        }

        IUnarySource IResult.ToSource()
        {
            return ToSource();
        }

        public IUnarySource<TOut> ToSource()
        {
            return Unary.Value(Output);
        }
    }
}