using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

namespace Smooth.Operands
{
    public static class Binary
    {
        static Binary()
        {
            MethodInfo Source2_MethodInfo = typeof(Binary)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Single(method => method.Name == "Source" && method.IsGenericMethodDefinition);
            GenericSourceMethodInfoFactory = (Type leftType, Type rightType) => Source2_MethodInfo.MakeGenericMethod(leftType, rightType);
        }

        private static readonly Func<Type, Type, MethodInfo> GenericSourceMethodInfoFactory;

        public static IBinarySource<L, R> OrDefault<L, R>(this IBinarySource<L, R> source)
        {
            return source ?? Source(ValueObject<L>.Initial, ValueObject<R>.Initial);
        }

        public static bool LeftValueEquals<L, R>(this IBinarySource<L, R> source, L value)
        {
            return ValueObject<L>.Equivalence.Equals(source.OrDefault().LeftOperand, value);
        }

        public static bool RightValueEquals<L, R>(this IBinarySource<L, R> source, R value)
        {
            return ValueObject<R>.Equivalence.Equals(source.OrDefault().RightOperand, value);
        }

        public static bool ValueEquals<L, R>(this IBinarySource<L, R> source, L left, R right)
        {
            return source.LeftValueEquals(left) && source.RightValueEquals(right);
        }

        public static Binary<L, R> Source<L, R>(L left, R right)
        {
            return new Binary<L, R>(left, right);
        }

        public static IBinarySource Source(Type leftType, object leftObject, Type rightType, object rightObject)
        {
            Contract.Requires<ArgumentNullException>(leftType != null && rightType != null);
            Contract.Requires<ArgumentNullException>(!(leftType.IsValueType && leftObject == null) && !(rightType.IsValueType && rightObject == null));
            Contract.Requires<ArgumentException>(leftType.IsInstanceOfType(leftObject) || leftObject == null);
            Contract.Requires<ArgumentException>(rightType.IsInstanceOfType(rightObject) || rightObject == null);
            Contract.Ensures(Contract.Result<IBinarySource>() != null);
            return (IBinarySource)GenericSourceMethodInfoFactory(leftType, rightType).Invoke(null, new object[] { leftObject, rightObject });
        }
        public static Binary<L,R> FromArray<L,R>(object[] array)
        {
            Contract.Requires<ArgumentNullException>(array != null);
            Contract.Requires<ArgumentException>(array.Length == 2);
            return Binary.Source((L)array[0],(R)array[1]);
        }
    }
}