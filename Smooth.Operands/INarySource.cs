using System;
using System.Collections;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Smooth.Operands
{
    [ContractClass(typeof(INarySourceContract))]
    public interface INarySource : ISource
    {
        IEnumerable Operands { get; }

        int Arity { get; }
    }

    [ContractClassFor(typeof(INarySource))]
    internal abstract class INarySourceContract : INarySource
    {
        bool IEquatable<INarySource>.Equals(INarySource other)
        {
            throw new NotImplementedException();
        }

        bool ISource.IsInitial
        {
            get { throw new NotImplementedException(); }
        }

        public INarySource ToNary()
        {
            throw new NotImplementedException();
        }

        IEnumerable INarySource.Operands
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable>() != null);
                return default(IEnumerable);
            }
        }

        int INarySource.Arity
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);
                Contract.Ensures(Contract.Result<int>() == ((INarySource)this).Operands.Cast<object>().Count());
                return default(int);
            }
        }
    }
}