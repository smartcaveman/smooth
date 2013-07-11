using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Smooth.ProcessModel
{
    public class Process : ICloneable, IObserver<ProcessState>, IDisposable, IStateful<ProcessState>
    {
        static Process()
        {
            Cloning = new { };
        }

        private static readonly object Cloning;

        public static Process New()
        {
            return new Process();
        }

        public static Process Load(Action action)
        {
            Process process = New();
            if (action != null)
                process.Running += action;
            return process;
        }

        public static Process Load(Action<Process> action)
        {
            Process process = New();
            if (action != null)
                process.Running += new ActionBinding<Process>(action, process);
            return process;
        }

        public static Process Execute(Action action)
        {
            Process process = Load(action);
            process.Run();
            return process;
        }

        public static Process Execute(Action<Process> action)
        {
            Process process = Load(action);
            process.Run();
            return process;
        }

        public static Process Error(Exception exception)
        {
            return Execute(x => { using (x) x.Current.OnError(exception); });
        }

        public static Process Void()
        {
            return Execute(() => { });
        }

        private Process()
        {
            State = ProcessState.Current;
        }

        public ProcessState State { get; private set; }

        public IEnumerable<Exception> Exceptions
        {
            get
            {
                if (exceptionStack == null)
                    yield break;
                foreach (var exception in exceptionStack)
                    yield return exception;
            }
        }

        private void Log(Exception exception)
        {
            Contract.Requires<ArgumentNullException>(exception != null);
            Contract.Assert(State.IsCurrent);
            if (!ReferenceEquals(exception, null))
                using (this)
                    (exceptionStack ?? (exceptionStack = new Stack<Exception>())).Push(exception);
        }

        private Stack<Exception> exceptionStack;

        private event Action running;

        public event Action Running
        {
            add
            {
                if (value != null)
                    lock (Cloning)
                        running += value;
            }
            remove
            {
                if (value != null)
                    lock (Cloning)
                        running -= value;
            }
        }

        private void Run()
        {
            if (State.IsCurrent)
                Current.OnNext(State);
        }

        internal IObserver<ProcessState> Current
        {
            get { return this.State.IsCurrent ? this : this.Clone(); }
        }

        void IObserver<ProcessState>.OnNext(ProcessState value)
        {
            if (!State.IsCurrent || ReferenceEquals(value, null)) return;
            if (value.IsCurrent)
            {
                Action run = this.running;
                if (run != null)
                    try
                    {
                        run();
                    }
                    catch (Exception exception)
                    {
                        Log(exception);
                    }
            }
            if (value.IsAbandoned)
                using (this)
                    return;
            Complete();
        }

        void IObserver<ProcessState>.OnError(Exception error)
        {
            Log(error);
        }

        void IObserver<ProcessState>.OnCompleted()
        {
            Complete();
        }

        void IDisposable.Dispose()
        {
            if (!State.IsCurrent) return;
            State = ProcessState.Abandoned;
        }

        private void Complete()
        {
            if (!State.IsCurrent) return;
            State = ProcessState.Completed;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public Process Clone()
        {
            Process clone = new Process();
            if (running != null)
                lock (Cloning)
                    if (running != null)
                        foreach (Action action in running.GetInvocationList().Cast<Action>())
                            clone.Running += action.Target is ActionBinding<Process> && this.Equals(((ActionBinding<Process>)action.Target).Target)
                                                 ? ((ActionBinding<Process>)action.Target).Rebind(clone)
                                                 : action;
            return clone;
        }
    }
}