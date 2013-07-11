using System.Collections.Generic;
using System.Linq;

namespace Smooth.Operands
{
    public static class Nary
    {
        public static INarySource<T> OrDefault<T>(this INarySource<T> source)
        {
            return source ?? default(Nary<T>);
        }

        public static Nary<T> Source<T>(IEnumerable<T> operands)
        {
            return new Nary<T>(operands);
        }

        public static Nary<T> Source<T>(params T[] operands)
        {
            return Source((operands ?? new T[0]).AsEnumerable());
        }

        public static bool ValuesEqual<T>(this INarySource<T> source, params T[] operands)
        {
            return source.ValuesEqual((operands ?? new T[0]).AsEnumerable());
        }

        public static bool ValuesEqual<T>(this INarySource<T> source, IEnumerable<T> operands)
        {
            return source.OrDefault().Equals(Source(operands ?? new T[0]));
        }
    }
}