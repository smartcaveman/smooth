using System;

namespace Smooth.Operands
{
    public static class Unary
    {
        public static Unary<T> Value<T>(T operand)
        {
            return new Unary<T>(operand);
        }

        public static IUnarySource<T> OrDefault<T>(this IUnarySource<T> source)
        {
            return source ?? Value(default(T));
        }

        public static bool ValueEquals<T>(this IUnarySource<T> source, T value)
        {
            return Unary<T>.OperandEqualityComparer.Equals(source.OrDefault().Operand, value);
        }

        public static IUnarySource<TNext> Select<T, TNext>(this IUnarySource<T> source, Func<T, TNext> map)
        {
            return new Unary<TNext>(map(source.OrDefault().Operand));
        }
    }
}