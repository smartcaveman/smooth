using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Smooth.Operands
{
    public struct Binary<L, R>
        : IBinarySource<L, R>
        , INarySource<object>
        , IEquatable<IBinarySource<L, R>>
        , IEquatable<IEnumerable>
        , IEquatable<Tuple<L, R>>
        , IEquatable<Binary<L, R>>
        , IEnumerable<object>
    {
        private readonly Unary<L> left;
        private readonly Unary<R> right;

        public Binary(L left, R right)
        {
            this.left = Unary.Value(left);
            this.right = Unary.Value(right);
        }

        public bool IsInitial
        {
            get { return Left.IsInitial && Right.IsInitial; }
        }


        public Unary<L> Left { get { return this.left; } }

        public Unary<R> Right { get { return this.right; } }

        IEnumerable INarySource.Operands { get { return this; } }

        public int Arity
        {
            get { return 2; }
        }

        public IEnumerable<object> Operands { get { return this; } }

        object IBinarySource.LeftOperand
        {
            get { return Left.Operand; }
        }

        object IBinarySource.RightOperand
        {
            get { return Right.Operand; }
        }

        public L LeftOperand
        {
            get { return Left.Operand; }
        }

        public R RightOperand
        {
            get { return Right.Operand; }
        }

        public override string ToString()
        {
            return string.Join("({0},{1})", Left.ToString().TrimStart('(').TrimEnd(')'), Right.ToString().TrimStart('(').TrimEnd(')'));
        }

        #region Iteration

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator<object> IEnumerable<object>.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IEnumerator<object> GetEnumerator()
        {
            yield return Left.Operand;
            yield return Right.Operand;
        }

        #endregion Iteration

        public override int GetHashCode()
        {
            return Left.GetHashCode() * Right.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null)) return IsInitial;
            if (obj is IBinarySource<L, R>) return Equals((IBinarySource<L, R>)obj);
            if (obj is INarySource) return Equals((INarySource)obj);
            if (obj is Tuple<L, R>) return Equals((Tuple<L, R>)obj);
            if (obj is IEnumerable) return Equals((IEnumerable)obj);
            return false;
        }

        public bool Equals(IBinarySource<L, R> other)
        {
            return (ReferenceEquals(other, null) || other.IsInitial) ? IsInitial : Left.ValueEquals(other.LeftOperand) && Right.ValueEquals(other.RightOperand);
        }

        public bool Equals(Tuple<L, R> other)
        {
            return ReferenceEquals(other, null)
                       ? IsInitial
                       : Left.Equals(Unary.Value(other.Item1)) && Right.Equals(Unary.Value(other.Item2));
        }

        public bool Equals(Binary<L, R> other)
        {
            return Left.Equals(other.Left) && Right.Equals(other.Right);
        }

        public bool Equals(INarySource other)
        {
            if ((ReferenceEquals(other, null) || other.IsInitial)) return IsInitial;
            return Equals(other.Operands);
        }

        public bool Equals(IEnumerable other)
        {
            if (ReferenceEquals(other, null)) return IsInitial;

            var list = other.Cast<object>().ToList();
            switch (list.Count)
            {
                case 0:
                    return IsInitial;
                case 1:
                    return false;
                case 2:
                    return list[0] is L && list[1] is R && Left.ValueEquals((L)list[0]) && Right.ValueEquals((R)list[1]);
                default:
                    return false;
            }
        }

        public static bool operator ==(Binary<L, R> x, Binary<L, R> y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Binary<L, R> x, Binary<L, R> y)
        {
            return x.Equals(y).Equals(false);
        }

        public static bool operator ==(Binary<L, R> x, object y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Binary<L, R> x, object y)
        {
            return x.Equals(y).Equals(false);
        }

        public static bool operator ==(object x, Binary<L, R> y)
        {
            return y.Equals(x);
        }

        public static bool operator !=(object x, Binary<L, R> y)
        {
            return y.Equals(x).Equals(false);
        }
    }
}