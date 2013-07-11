using System;
using System.Diagnostics.Contracts;
using Smooth.Operands;
using Smooth.ProcessModel;

namespace Smooth.OperationModel
{
    public class Operation<TIn, TOut> : IEquatable<Operation<TIn, TOut>>
        where TIn : ISource
    {
        private readonly IOperator<TIn, TOut> @operator;
        private readonly TIn input;

        public Operation(IOperator<TIn, TOut> @operator, TIn input)
        {
            Contract.Requires<ArgumentNullException>(!ReferenceEquals(@operator, null));
            Contract.Requires<ArgumentNullException>(!ReferenceEquals(input, null));
            this.@operator = @operator;
            this.input = input;
        }

        public TIn Input
        {
            get { return input; }
        }

        public IOperator<TIn, TOut> Operator
        {
            get { return @operator; }
        }

        public IOperationResult<TOut> CreateResult(ProcessResult processResult, TOut output)
        {
            return new OperationResult<TOut>(processResult, Operator, output);
        }

        private OperationResult<TOut> PreConditionFailed()
        {
            return new OperationResult<TOut>(ProcessResult.PreConditionFailed(), Operator, ValueObject<TOut>.Initial);
        }

        private OperationResult<TOut> PreConditionException(Exception exception)
        {
            return new OperationResult<TOut>(ProcessResult.PreConditionException(exception), Operator, ValueObject<TOut>.Initial);
        }

        private OperationResult<TOut> OperationException(Exception exception)
        {
            return new OperationResult<TOut>(ProcessResult.ProcessException(exception), Operator, ValueObject<TOut>.Initial);
        }

        private OperationResult<TOut> Completed(TOut output)
        {
            return new OperationResult<TOut>(ProcessResult.Completed(), Operator, output);
        }

        private OperationResult<TOut> PostConditionFailed(TOut output)
        {
            return new OperationResult<TOut>(ProcessResult.PostConditionFailed(), Operator, output);
        }

        private OperationResult<TOut> PostConditionException(TOut output, Exception exception)
        {
            return new OperationResult<TOut>(ProcessResult.PostConditionException(exception), Operator, output);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Operation<TIn, TOut>);
        }

        public bool Equals(Operation<TIn, TOut> other)
        {
            return !ReferenceEquals(other, null) && Operator.Symbol.Equals(other.Operator.Symbol) &&
                   Input.Equals(other.Input);
        }

        public override int GetHashCode()
        {
            return Operator.GetHashCode() * 2 + Input.GetHashCode() * 3;
        }

        public static bool operator ==(Operation<TIn, TOut> left, Operation<TIn, TOut> right)
        {
            return ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.Equals(right);
        }

        public static bool operator !=(Operation<TIn, TOut> left, Operation<TIn, TOut> right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return Operator.ToString() + Input.ToString();
        }
    }
}