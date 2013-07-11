using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Smooth.Operands
{
    public struct Nary<T>
        : INarySource<T>
        , IEquatable<IEnumerable>
        , IEquatable<IStructuralEquatable>
        , IEquatable<Nary<T>>
        , IEnumerable<T>
    {
        static Nary()
        {
            Default = default(Nary<T>);
        }

        public static readonly Nary<T> Default;

        private static bool IsNullTerm(object x)
        {
            return ReferenceEquals(x, null) || x is T && ValueObject<T>.Equivalence.Equals((T)x);
        }

        private readonly IList<T> operands;

        public Nary(IEnumerable<T> operands)
        {
            this.operands = Array.AsReadOnly(operands == null ? new T[0] : operands.ToArray());
        }

        INarySource ISource.ToNary()
        {
            return this;
        }

        public override string ToString()
        {
            return '(' + string.Join(",", Operands) + ')';
        }

        public bool IsInitial
        {
            get
            {
                return Arity == 0
                       || Operands.All(x => ValueObject<T>.Equivalence.Equals(x, ValueObject<T>.Initial));
            }
        }

        public int Arity
        {
            get { return this.operands.Count; }
        }

        IEnumerable INarySource.Operands
        {
            get { return Operands; }
        }

        public IEnumerable<T> Operands
        {
            get
            {
                foreach (var operand in operands)
                    yield return operand;
            }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return Operands.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Operands.GetEnumerator();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return IsInitial;
            return this.Equal<INarySource>(obj)
                   || this.Equal<IEnumerable>(obj)
                   || this.Equal<IStructuralEquatable>(obj);
        }

        bool IEquatable<IStructuralEquatable>.Equals(IStructuralEquatable other)
        {
            IEnumerable enumerable;
            return TryEnumerate(other, out enumerable) && this.Equal<IEnumerable>(enumerable);
        }

        public bool TryEnumerate(IStructuralEquatable obj, out IEnumerable enumerable)
        {
            if (obj is Tuple<T>)
            {
                var tuple = (Tuple<T>)obj;
                enumerable = (new[] { tuple.Item1 });
                return true;
            }
            if (obj is Tuple<T, T>)
            {
                var tuple = (Tuple<T, T>)obj;
                enumerable = (new T[] { tuple.Item1, tuple.Item2 });
                return true;
            }
            if (obj is Tuple<T, T, T>)
            {
                var tuple = (Tuple<T, T, T>)obj;
                enumerable = (new T[] { tuple.Item1, tuple.Item2, tuple.Item3 });
                return true;
            }
            if (obj is Tuple<T, T, T, T>)
            {
                var tuple = (Tuple<T, T, T, T>)obj;
                enumerable = (new T[] { tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4 });
                return true;
            }
            if (obj is Tuple<T, T, T, T, T>)
            {
                var tuple = (Tuple<T, T, T, T, T>)obj;
                enumerable = (new T[] { tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5 });
                return true;
            }
            if (obj is Tuple<T, T, T, T, T, T>)
            {
                var tuple = (Tuple<T, T, T, T, T, T>)obj;
                enumerable = (new T[] { tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6 });
                return true;
            }
            if (obj is Tuple<T, T, T, T, T, T, T>)
            {
                var tuple = (Tuple<T, T, T, T, T, T, T>)obj;
                enumerable = (new T[] { tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7 });
                return true;
            }
            enumerable = null;
            return false;
        }

        public bool Equals(Nary<T> other)
        {
            return IsInitial ? other.IsInitial : !other.IsInitial && Equals(other.Operands);
        }

        bool IEquatable<INarySource>.Equals(INarySource other)
        {
            if (ReferenceEquals(other, null) || other.IsInitial) return IsInitial;
            return this.Equal<IEnumerable>(other.Operands);
        }

        bool IEquatable<IEnumerable>.Equals(IEnumerable other)
        {
            if (IsInitial)
            {
                if (ReferenceEquals(other, null)) return true;
                IEnumerator enumerator = other.GetEnumerator();
                while (enumerator.MoveNext())
                    if (!IsNullTerm(enumerator.Current))
                        return false;
                return true;
            }
            if (ReferenceEquals(other, null)) return false;
            List<object> local = Operands.Cast<object>().ToList();
            List<object> comparand = other.Cast<object>().ToList();
            if (Arity == comparand.Count) return local.SequenceEqual(comparand);
            return (Arity < comparand.Count)
                ? comparand.Skip(local.Count).All(IsNullTerm) && comparand.Take(local.Count).SequenceEqual(local)
                : local.Skip(comparand.Count).All(IsNullTerm) && local.Take(comparand.Count).SequenceEqual(comparand);
        }

        public override int GetHashCode()
        {
            switch (Arity)
            {
                case 0:
                    return Nullary.Value.GetHashCode();
                case 1:
                    return Unary.Source(Operands.Single()).GetHashCode();
                case 2:
                    return Binary.Source(Operands.First(), Operands.Skip(1).Single()).GetHashCode();
                default:
                    return Operands.Select(x => x.GetHashCode()).Aggregate(17, (a, b) => (a * 31) + b);
            }
        }

        public static bool operator ==(Nary<T> x, Nary<T> y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Nary<T> x, Nary<T> y)
        {
            return x.Equals(y).Equals(false);
        }

        public static bool operator ==(Nary<T> x, object y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Nary<T> x, object y)
        {
            return x.Equals(y).Equals(false);
        }

        public static bool operator ==(object x, Nary<T> y)
        {
            return y.Equals(x);
        }

        public static bool operator !=(object x, Nary<T> y)
        {
            return y.Equals(x).Equals(false);
        }

        public static implicit operator Nary<T>(Binary<T, T> s)
        {
            return new Nary<T>(s.Operands.Cast<T>());
        }

        public static implicit operator Nary<T>(Unary<T> s)
        {
            return new Nary<T>(s.Operands);
        }

        public static implicit operator Nary<T>(Nullary s)
        {
            return new Nary<T>(s.Cast<T>());
        }
    }
}