using System;
using System.Linq;
using Smooth.OperationModel;

namespace Smooth.ProcessModel
{
    public sealed class ProcessResult : Result
    {
        private readonly Exception exception;
        private bool? postConditionOutput;
        private bool? preConditionOutput;

        private ProcessResult(
            Process process,
            bool? preConditionOutput = default(bool?),
            bool? postConditionOutput = default(bool?))
            : base(ref process)
        {
            this.preConditionOutput = preConditionOutput;
            this.postConditionOutput = postConditionOutput;
            Exception[] exceptions = process.Exceptions.ToArray();
            switch (exceptions.Length)
            {
                case 0:
                    break;

                case 1:
                    this.exception = exceptions[0];
                    break;

                default:
                    this.exception = new AggregateException(exceptions).Flatten();
                    break;
            }
        }

        public override bool HasPostValidationError
        {
            get { return postConditionOutput.HasValue && !postConditionOutput.Value; }
        }

        public override bool HasPreValidationError
        {
            get { return preConditionOutput.HasValue && !preConditionOutput.Value; }
        }

        public override Exception Exception
        {
            get { return exception; }
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

        public static ProcessResult PreConditionFailed()
        {
            return new ProcessResult(Process.New(), preConditionOutput: false);
        }

        public static ProcessResult PreConditionException(Exception exception)
        {
            return new ProcessResult(Process.Error(exception));
        }

        public static ProcessResult ProcessException(Exception exception)
        {
            return new ProcessResult(Process.Error(exception), preConditionOutput: true);
        }

        public static ProcessResult Completed()
        {
            return new ProcessResult(Process.Void(), preConditionOutput: true, postConditionOutput: true);
        }

        public static ProcessResult PostConditionFailed()
        {
            return new ProcessResult(Process.New(), preConditionOutput: true, postConditionOutput: false);
        }

        public static ProcessResult PostConditionException(Exception exception)
        {
            return new ProcessResult(Process.Error(exception ?? new ApplicationException()), preConditionOutput: true);
        }

        public static ProcessResult PostConditionException<TException>()
            where TException : Exception, new()
        {
            return PostConditionException(new TException());
        }
    }
}