using System;
using Smooth.Operands;

namespace Smooth.Strategies
{
    public class BinaryFunctionStrategy<L, R, T> : FunctionStrategy<Binary<L, R>, T>
    {
        public BinaryFunctionStrategy(Func<L, R, T> operation, Predicate<Binary<L, R>> preCondition = null, Predicate<T> postCondition = null, Predicate<FunctionContext<Binary<L, R>, T>> invariant = null)
            : base(x => operation(x.LeftOperand, x.RightOperand), preCondition, postCondition, invariant)
        {
        }

        public BinaryFunctionStrategy(Func<Func<L, R, T>> loadOperation, Predicate<Binary<L, R>> preCondition = null, Predicate<T> postCondition = null, Predicate<FunctionContext<Binary<L, R>, T>> invariant = null)
            : base(() => {
                var operation = loadOperation();
                return x => operation(x.LeftOperand, x.RightOperand);
            }, preCondition, postCondition, invariant)
        {
        }
    }
}