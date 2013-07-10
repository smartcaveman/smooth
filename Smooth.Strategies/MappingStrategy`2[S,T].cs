using System;
using Smooth.Operands;

namespace Smooth.Strategies
{
    public class MappingStrategy<S, T> : FunctionStrategy<Unary<S>, T>
    {
        public MappingStrategy(Func<S, T> operation, Predicate<Unary<S>> preCondition = null, Predicate<T> postCondition = null, Predicate<FunctionContext<Unary<S>, T>> invariant = null)
            : base(x => operation(x.Operand), preCondition, postCondition, invariant)
        {
        }

        public MappingStrategy(Func<Func<S, T>> loadOperation, Predicate<Unary<S>> preCondition = null, Predicate<T> postCondition = null, Predicate<FunctionContext<Unary<S>, T>> invariant = null)
            : base(() => {
                    var operation = loadOperation();
                    return x => operation(x.Operand);
                }, preCondition, postCondition, invariant)
        {
        }
    }
}