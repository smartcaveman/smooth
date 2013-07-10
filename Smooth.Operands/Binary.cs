namespace Smooth.Operands
{
    public static class Binary
    {
        public static IBinarySource<L, R> OrDefault<L, R>(this IBinarySource<L, R> source)
        {
            return source ?? Value(default(L), default(R));
        }

        public static bool LeftValueEquals<L, R>(this IBinarySource<L, R> source, L value)
        {
            return Unary<L>.OperandEqualityComparer.Equals(source.OrDefault().LeftOperand, value);
        }

        public static bool RightValueEquals<L, R>(this IBinarySource<L, R> source, R value)
        {
            return Unary<R>.OperandEqualityComparer.Equals(source.OrDefault().RightOperand, value);
        }

        public static bool ValueEquals<L, R>(this IBinarySource<L, R> source, L left, R right)
        {
            return source.LeftValueEquals(left) && source.RightValueEquals(right);
        }

        public static Binary<L, R> Value<L, R>(L left, R right)
        {
            return new Binary<L, R>(left, right);
        }
    }
}