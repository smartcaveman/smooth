using System;
using System.Diagnostics.Contracts;

namespace Smooth.ProcessModel
{
    public abstract class Result : IResult, IProcessState
    {
        private readonly DateTimeOffset timeStamp;
        private readonly ProcessState processState;

        protected internal Result(ProcessState processState)
        {
            Contract.Requires<ArgumentNullException>(!ReferenceEquals(processState, null));
            this.processState = processState;
        }

        protected internal Result(ref Process process)
        {
            using (process ?? (process = Process.New()))
            {
                this.timeStamp = DateTimeOffset.UtcNow;
            }
            this.processState = process.State;
        }

        internal ProcessState ProcessState { get { return this.processState; } }

        bool IProcessState.IsCurrent { get { return false; } }

        public bool IsAbandoned { get { return processState.IsAbandoned; } }

        public bool IsCompleted { get { return processState.IsCompleted; } }

        public bool HasError { get { return HasValidationError || HasException; } }

        public bool HasException { get { return !ReferenceEquals(Exception, null); } }

        public abstract bool HasPostValidationError { get; }

        public abstract bool HasPreValidationError { get; }

        public bool HasValidationError { get { return HasPostValidationError || HasPreValidationError; } }

        public abstract Exception Exception { get; }

        public virtual DateTimeOffset TimeStamp { get { return this.timeStamp; } }
    }
}