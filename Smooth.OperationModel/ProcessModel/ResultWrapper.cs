using System;
using System.Diagnostics.Contracts;

namespace Smooth.ProcessModel
{
    public abstract class ResultWrapper : Result
    {
        private readonly ProcessResult core;

        public ResultWrapper(ProcessResult core)
            : base(core.ProcessState)
        {
            Contract.Requires<ArgumentNullException>(!ReferenceEquals(core, null));
            this.core = core;
        }

        public sealed override bool HasPostValidationError
        {
            get { return core.HasPostValidationError; }
        }

        public sealed override bool HasPreValidationError
        {
            get { return core.HasPreValidationError; }
        }

        public sealed override Exception Exception
        {
            get { return core.Exception; }
        }

        public sealed override DateTimeOffset TimeStamp
        {
            get { return core.TimeStamp; }
        }
    }
}