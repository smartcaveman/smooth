using System;
using System.Diagnostics.Contracts;

namespace Smooth.ProcessModel
{
    public class ActionBinding<T>
    {
        public ActionBinding(Action<T> action, T target)
        {
            Contract.Requires<ArgumentNullException>(action != null);
            this.Action = action;
            this.Target = target;
        }

        public Action<T> Action { get; private set; }

        public T Target { get; private set; }

        public void Execute()
        {
            Action(Target);
        }

        public ActionBinding<T> Rebind(T newTarget)
        {
            return Equals(Target, newTarget) ? this : new ActionBinding<T>(Action, newTarget);
        }

        public static implicit operator Action(ActionBinding<T> actionBinding)
        {
            return actionBinding == null ? Void : actionBinding.Execute;
        }

        private static readonly Action Void = () => { };
    }
}