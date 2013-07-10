using System;
using Smooth.Operands;

namespace Smooth.Strategies
{
    public class ConstantStrategy<T> : FunctionStrategy<Nullary, T>
    {
        public ConstantStrategy(T value, Predicate<T> condition = null)
            : base(x => value, null, condition, null)
        {
        }

        public ConstantStrategy(Func<T> value, Predicate<T> condition = null)
            : base(() => x => value(), null, condition, null)
        {
        }

        public T Value
        {
            get { return this[Nullary.Value()]; }
        }
    }
}