using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Smooth.Operands
{
    public struct Unary<T>
        : IUnarySource<T>
        , INarySource<T>
        , IEquatable<IUnarySource<T>>
        , IEquatable<IEnumerable>
        , IEquatable<Tuple<T>>
        , IEquatable<Unary<T>>
        , IEnumerable<T>
    {
        private readonly T operand;

        public Unary(T operand)
        {
            this.operand = operand;
        }

        public static IEqualityComparer<T> OperandEqualityComparer
        {
            get { return EqualityComparer<T>.Default; }
        }

        public bool IsInitial
        {
            get { return OperandEqualityComparer.Equals(Operand, default(T)); }
        }

        object IUnarySource.Operand
        {
            get { return Operand; }
        }

        IEnumerable INarySource.Operands
        {
            get { return Operands; }
        }

        public int Arity
        {
            get { return 1; }
        }

        public T Operand
        {
            get { return operand; }
        }

        public IEnumerable<T> Operands
        {
            get { yield return Operand; }
        }

        public override string ToString()
        {
            return string.Format("({0})", ReferenceEquals(Operand, null) ? "null" : Operand.ToString());
        }

        #region Enumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IEnumerator<T> GetEnumerator()
        {
            yield return Operand;
        }

        #endregion Enumerable

        #region Equality

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null)) return IsInitial;
            if (obj is IUnarySource<T>) return Equals((IUnarySource<T>)obj);
            if (obj is INarySource) return Equals((INarySource)obj);
            if (obj is Tuple<T>) return Equals((Tuple<T>)obj);
            if (obj is IEnumerable) return Equals((IEnumerable)obj);
            return false;
        }

        public bool Equals(Unary<T> other)
        {
            return OperandEqualityComparer.Equals(Operand, other.Operand);
        }

        public bool Equals(Tuple<T> other)
        {
            return ReferenceEquals(other, null) ? IsInitial : OperandEqualityComparer.Equals(Operand, other.Item1);
        }

        public bool Equals(IUnarySource<T> other)
        {
            return (ReferenceEquals(other, null) || other.IsInitial) ? IsInitial : OperandEqualityComparer.Equals(Operand, other.Operand);
        }

        public bool Equals(INarySource other)
        {
            return (ReferenceEquals(other, null) || other.IsInitial) ? IsInitial : Equals(other.Operands);
        }

        public bool Equals(IEnumerable other)
        {
            if (ReferenceEquals(other, null)) return IsInitial;
            var list = other.Cast<object>().ToList();
            switch (list.Count)
            {
                case 0:
                    return IsInitial;
                case 1:
                    return list[0] is T && OperandEqualityComparer.Equals(Operand, (T)list[0]);
                default:
                    return false;
            }
        }

        public override int GetHashCode()
        {
            return IsInitial ? 0 : OperandEqualityComparer.GetHashCode(Operand);
        }

        public static bool operator ==(Unary<T> x, Unary<T> y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Unary<T> x, Unary<T> y)
        {
            return !(x == y);
        }

        public static bool operator ==(Unary<T> x, object y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Unary<T> x, object y)
        {
            return !(x == y);
        }

        public static bool operator ==(object x, Unary<T> y)
        {
            return (y == x);
        }

        public static bool operator !=(object x, Unary<T> y)
        {
            return !(y == x);
        }

        #endregion Equality

        public static Unary<T> FromValue(T operand)
        {
            return new Unary<T>(operand);
        }

        public static explicit operator Unary<T>(T domain)
        {
            return FromValue(domain);
        }

        public static implicit operator Unary<T>(Tuple<T> tuple)
        {
            return Unary<T>.FromValue(ReferenceEquals(tuple, null) ? default(T) : tuple.Item1);
        }
    }
}