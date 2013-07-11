using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Smooth.Operands
{
    [ContractClass(typeof(INarySourceContract<>))]
    public interface INarySource<out T> : INarySource
    {
        new IEnumerable<T> Operands { get; }
    }

    [ContractClassFor(typeof(INarySource<>))]
    internal abstract class INarySourceContract<T> : INarySource<T>
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
            get { throw new NotImplementedException(); }
        }

        IEnumerable<T> INarySource<T>.Operands
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);
                Contract.Ensures(Contract.Result<IEnumerable<T>>().Cast<object>().SequenceEqual(((INarySource)this).Operands.Cast<object>()));
                return default(IEnumerable<T>);
            }
        }

        int INarySource.Arity
        {
            get { throw new NotImplementedException(); }
        }
    }
}