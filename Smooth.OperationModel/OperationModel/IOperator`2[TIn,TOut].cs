namespace Smooth.OperationModel
{
    using Operands;

    public interface IOperator<in TIn, out TOut> : IOperator
        where TIn : ISource
    {
        IResult<TOut> Operate(TIn input);
    }
}