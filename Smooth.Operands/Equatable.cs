using System;

namespace Smooth.Operands
{
    public static class Equatable
    {
        public static bool Equal<T>(this IEquatable<T> equatable, object to)
        {
            return equatable != null && to is T && equatable.Equals((T)to);
        }
    }
}