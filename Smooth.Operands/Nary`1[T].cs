using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Smooth.Operands
{
    public struct Nary<T>
        : INarySource<T>
        , IEquatable<INarySource<T>>
        , IEquatable<IEnumerable>
        , IEquatable<Nary<T>>
        , IEnumerable<T>
    {
        private readonly IList<T> operands;

        public Nary(IEnumerable<T> operands)
        {
            this.operands = Array.AsReadOnly((operands ?? Enumerable.Empty<T>()).ToArray());
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
                       || Operands.All(x => Unary<T>.OperandEqualityComparer.Equals(x, default(T)));
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
            if (obj is INarySource) return Equals((INarySource)obj);
            if (obj is IEnumerable) return Equals((IEnumerable)obj);
            if (obj is IStructuralEquatable)
            {
                IEnumerable enumerable;
                return TryEnumerate((IStructuralEquatable)obj, out enumerable) && Equals(enumerable);
            }
            return false;
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

        public bool Equals(INarySource other)
        {
            if (ReferenceEquals(other, null) || other.IsInitial) return IsInitial;
            if (Arity != other.Arity) return false;
            return Equals(other.Operands);
        }

        public bool Equals(INarySource<T> other)
        {
            if (ReferenceEquals(other, null) || other.IsInitial) return IsInitial;
            if (Arity != other.Arity) return false;
            return Equals(other.Operands);
        }

        public bool Equals(IEnumerable other)
        {
            if (ReferenceEquals(other, null)) return IsInitial;
            var comparand = other.Cast<object>().ToList();
            if (Arity != comparand.Count)
                return IsInitial && comparand.TrueForAll(x => ReferenceEquals(x, null));
            return Operands.Cast<object>().SequenceEqual(comparand);
        }

        public override int GetHashCode()
        {
            switch (Arity)
            {
                case 0:
                    return Nullary.Value().GetHashCode();
                case 1:
                    return Unary.Value(Operands.Single()).GetHashCode();
                case 2:
                    return Binary.Value(Operands.First(), Operands.Skip(1).Single()).GetHashCode();
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
            return new Nary<T>(s.Cast<T>());
        }

        public static implicit operator Nary<T>(Unary<T> s)
        {
            return new Nary<T>(s.Cast<T>());
        }

        public static implicit operator Nary<T>(Nullary s)
        {
            return new Nary<T>(s.Cast<T>());
        }
    }
}