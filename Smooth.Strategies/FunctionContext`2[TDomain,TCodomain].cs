using System.Collections;

namespace Smooth.Strategies
{
    public class FunctionContext<TDomain, TCodomain> : IEnumerable
    {
        private readonly TDomain domain;

        public FunctionContext(TDomain domain)
        {
            this.domain = domain;
        }

        public TDomain Domain
        {
            get { return domain; }
        }

        public TCodomain Codomain
        {
            get;
            set;
        }

        public IEnumerator GetEnumerator()
        {
            yield return Domain;
            yield return Codomain;
        }
    }
}