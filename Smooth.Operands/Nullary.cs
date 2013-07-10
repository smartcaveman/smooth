using System;
using System.Collections;
using System.Collections.Generic;

namespace Smooth.Operands
{
    public struct Nullary : INullarySource, INarySource<object>, IEquatable<IEnumerable>, IEquatable<Nullary>, IEnumerable<object>
    {
        public static Nullary Value()
        {
            return default(Nullary);
        }

        public int Arity
        {
            get { return 0; }
        }

        public bool IsInitial
        {
            get { return true; }
        }

        public INarySource<object> Nary()
        {
            return this;
        }

        public override bool Equals(object obj)
        {
            if (obj is Nullary) return true;
            if (ReferenceEquals(obj, null)) return true;
            if (obj is IEnumerable) return Equals((IEnumerable)obj);
            if (obj is INarySource) return Equals((INarySource)obj);
            return false;
        }

        public bool Equals(INarySource other)
        {
            return ReferenceEquals(other, null) || other.IsInitial || this.Equals(other.Operands);
        }

        public bool Equals(IEnumerable other)
        {
            return ReferenceEquals(other, null) || !other.GetEnumerator().MoveNext();
        }

        public override int GetHashCode()
        {
            return 0;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield break;
        }

        public bool Equals(Nullary other)
        {
            return true;
        }

        IEnumerator<object> IEnumerable<object>.GetEnumerator()
        {
            yield break;
        }

        public override string ToString()
        {
            return "()";
        }

        IEnumerable INarySource.Operands
        {
            get { yield break; }
        }

        IEnumerable<object> INarySource<object>.Operands
        {
            get { yield break; }
        }

        public static bool operator ==(Nullary a, Nullary b)
        {
            return true;
        }

        public static bool operator !=(Nullary a, Nullary b)
        {
            return false;
        }

        public static bool operator ==(Nullary a, object b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Nullary a, object b)
        {
            return a.Equals(b).Equals(false);
        }

        public static bool operator ==(object a, Nullary b)
        {
            return b.Equals(a);
        }

        public static bool operator !=(object a, Nullary b)
        {
            return b.Equals(a).Equals(false);
        }
    }
}