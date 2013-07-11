using System;
using System.Collections.Generic;

namespace Smooth.Operands
{
    public static class ValueObject<T>
    {
        private static IEqualityComparer<T> _equivalence;
        private static Func<T> _initial;

        public static IEqualityComparer<T> Equivalence
        {
            get { return _equivalence ?? EqualityComparer<T>.Default; }
        }

        public static T Initial
        {
            get { return _initial == null ? default(T) : _initial(); }
        }

        public static void Configure(IEqualityComparer<T> equivalence)
        {
            _equivalence = equivalence;
        }

        public static void Configure(Func<T> initial)
        {
            _initial = initial;
        }
    }
}