using System;
using Smooth.Operands;
using Smooth.ProcessModel;

namespace Smooth.OperationModel
{
    public class OperationResult<TOut> : ResultWrapper, IOperationResult<TOut>
    {
        private readonly ProcessResult result;
        private readonly IOperator<TOut> @operator;
        private readonly TOut output;

        protected internal OperationResult(ProcessResult result, IOperator<TOut> @operator, TOut output)
            : base(result)
        {
            this.result = result;
            this.@operator = @operator;
            this.output = output;
        }

        IOperator IOperationResult.Operator
        {
            get { return Operator; }
        }

        public IOperator<TOut> Operator
        {
            get { return this.@operator; }
        }

        object IOperationResult.Output
        {
            get { return Output; }
        }

        public TOut Output
        {
            get { return this.output; }
        }

        public IOperationResult<TNext> Select<TNext>(Func<TOut, TNext> map)
        {
            throw new NotImplementedException();
            /*
            return new OperationResult<TNext>(this.result, this.@operator, map(Output));
            return new ProcessResult(preConditionOutput,
                                             ReferenceEquals(Output, null) ? default(TNext) : selector(Output),
                                             postConditionOutput, Exception);
             * */
        }

        IUnarySource IOperationResult.ToSource()
        {
            return ToSource();
        }

        public IUnarySource<TOut> ToSource()
        {
            return Unary.Source<TOut>(Output);
        }
    }
}