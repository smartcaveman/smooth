namespace Smooth.Strategies
{
    public abstract class Strategy<TDelegate> : IStrategy<TDelegate>
        where TDelegate : class
    {
        private TDelegate invoker;

        protected TDelegate OriginalDelegate
        {
            get
            {
                return invoker ?? (invoker = ResolveOriginalDelegate());
            }
        }

        protected virtual TDelegate InterceptedDelegate
        {
            get { return null; }
        }
         
        protected abstract TDelegate ResolveOriginalDelegate();


        public TDelegate Delegate
        {
            get { return InterceptedDelegate ?? OriginalDelegate; }
        }
    }
}