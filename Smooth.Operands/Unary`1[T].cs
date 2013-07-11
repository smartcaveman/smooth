using System;
using System.Collections;
using System.Collections.Generic;

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
        static Unary()
        {
            Default = default(Unary<T>);
        }

        public static readonly Unary<T> Default;

        private readonly T operand;

        public Unary(T operand)
        {
            this.operand = operand;
        }

        INarySource ISource.ToNary()
        {
            return this;
        }

        public bool IsInitial
        {
            get { return ValueObject<T>.Equivalence.Equals(Operand, ValueObject<T>.Initial); }
        }

        object IUnarySource.Operand
        {
            get { return Operand; }
        }

        private static bool IsUnary1Struct(Type type)
        {
            return type != null && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Unary<>);
        }

        public IBinarySource With(IUnarySource other)
        {
            if (other == null) return this.With(Unary<object>.Default);
            Type otherType = other.GetType();
            return IsUnary1Struct(otherType)
                ? Binary.Source(typeof(T), Operand, otherType.GetGenericArguments()[0], other.Operand)
                : other.With(this).Reverse();
        }

        public IBinarySource<T, R> With<R>(IUnarySource<R> other)
        {
            return other is Unary<R> ? this.With((Unary<R>)other) : other.With(this).Reverse();
        }

        public Binary<T, R> With<R>(Unary<R> other)
        {
            return new Binary<T, R>(this, other);
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
            return this.Equal<IUnarySource<T>>(obj)
                   || this.Equal<INarySource>(obj)
                   || this.Equal<Tuple<T>>(obj)
                   || this.Equal<IEnumerable>(obj);
        }

        public bool Equals(Unary<T> other)
        {
            return ValueObject<T>.Equivalence.Equals(Operand, other.Operand);
        }

        bool IEquatable<Tuple<T>>.Equals(Tuple<T> other)
        {
            if (ReferenceEquals(other, null)) return IsInitial;
            return ValueObject<T>.Equivalence.Equals(Operand, other.Item1);
        }

        bool IEquatable<IUnarySource<T>>.Equals(IUnarySource<T> other)
        {
            if (ReferenceEquals(other, null) || other.IsInitial) return IsInitial;
            return ValueObject<T>.Equivalence.Equals(Operand, other.Operand);
        }

        bool IEquatable<INarySource>.Equals(INarySource other)
        {
            if (ReferenceEquals(other, null) || other.IsInitial) return IsInitial;
            return Equals(other.Operands);
        }

        bool IEquatable<IEnumerable>.Equals(IEnumerable other)
        {
            if (ReferenceEquals(other, null)) return IsInitial;
            bool equal = false;
            IEnumerator enumerator = other.GetEnumerator();
            if (enumerator.MoveNext())
            {
                object current = enumerator.Current;
                if (!enumerator.MoveNext())
                    equal = current is T && ValueObject<T>.Equivalence.Equals(Operand, (T)current);
            }
            else
                equal = IsInitial;
            if (enumerator is IDisposable) ((IDisposable)enumerator).Dispose();
            return equal;
        }

        public override int GetHashCode()
        {
            return IsInitial ? 0 : ValueObject<T>.Equivalence.GetHashCode(Operand);
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

        #region Conversions

        public static explicit operator Unary<T>(T domain)
        {
            return new Unary<T>(domain);
        }

        public static implicit operator Unary<T>(Tuple<T> tuple)
        {
            return ReferenceEquals(tuple, null) ? Default : new Unary<T>(tuple.Item1);
        }

        #endregion Conversions
    }
}