using System;

namespace Smooth.ProcessModel
{
    public sealed class ProcessState : IEquatable<ProcessState>, IObservable<ProcessState>, IProcessState
    {
        private readonly bool? completion;

        static ProcessState()
        {
            current = new ProcessState(null);
            abandoned = new ProcessState(false);
            completed = new ProcessState(true);
        }

        private static readonly ProcessState current, abandoned, completed;

        private ProcessState(bool? completion)
        {
            this.completion = completion;
        }

        public static ProcessState Current
        {
            get { return current; }
        }

        public static ProcessState Abandoned
        {
            get { return abandoned; }
        }

        public static ProcessState Completed
        {
            get { return completed; }
        }

        public bool IsCurrent { get { return !completion.HasValue; } }

        public bool IsAbandoned { get { return completion.HasValue && !completion.Value; } }

        public bool IsCompleted { get { return completion.HasValue && completion.Value; } }

        public ProcessState Update(Process process)
        {
            if (process == null) return Abandoned;
            ((IObserver<ProcessState>)(process)).OnNext(this);
            return process.State;
        }

        IDisposable IObservable<ProcessState>.Subscribe(IObserver<ProcessState> observer)
        {
            observer.OnNext(this);
            return observer as IDisposable ?? Process.New();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ProcessState);
        }

        public bool Equals(ProcessState other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return IsCurrent
                       ? 0
                       : IsCompleted
                             ? 1
                             : -1;
        }

        public static bool operator ==(ProcessState x, ProcessState y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;
            return x.completion.HasValue
                       ? y.completion.HasValue && x.completion.Value.Equals(y.completion.Value)
                       : !y.completion.HasValue;
        }

        public static bool operator !=(ProcessState x, ProcessState y)
        {
            return !(x == y);
        }

        public static bool operator ==(ProcessState x, object y)
        {
            return y is ProcessState && x == (ProcessState)y;
        }

        public static bool operator !=(ProcessState x, object y)
        {
            return !(x == y);
        }

        public static bool operator ==(object x, ProcessState y)
        {
            return y == x;
        }

        public static bool operator !=(object x, ProcessState y)
        {
            return !(x == y);
        }
    }
}