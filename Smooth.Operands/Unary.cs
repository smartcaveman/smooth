using System;
using System.Diagnostics.Contracts;

namespace Smooth.Operands
{
    public static class Unary
    {
        public static Unary<T> Source<T>(T operand)
        {
            return new Unary<T>(operand);
        }

        public static IUnarySource<T> OrDefault<T>(this IUnarySource<T> source)
        {
            return source ?? Source(ValueObject<T>.Initial);
        }

        public static bool ValueEquals<T>(this IUnarySource<T> source, T value)
        {
            return ValueObject<T>.Equivalence.Equals(source.OrDefault().Operand, value);
        }

        public static IUnarySource<TNext> Select<T, TNext>(this IUnarySource<T> source, Func<T, TNext> map)
        {
            Contract.Requires<ArgumentNullException>(map != null);
            return new Unary<TNext>(map(source.OrDefault().Operand));
        }

        public static Unary<T> FromArray<T>(object[] array)
        {
            Contract.Requires<ArgumentNullException>(array != null);
            Contract.Requires<ArgumentException>(array.Length == 1);
            return Unary.Source((T)array[0]); 
        }
    }
}