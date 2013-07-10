namespace Smooth.Operands
{
    public interface IBinarySource : ISource
    {
        object LeftOperand { get; }

        object RightOperand { get; }
    }
}