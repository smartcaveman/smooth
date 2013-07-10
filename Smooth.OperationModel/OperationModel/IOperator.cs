using Smooth.Operands;

namespace Smooth.OperationModel
{
    public interface IOperator
    {
        string Symbol { get; }

        IResult Operate(ISource input);
    }
}