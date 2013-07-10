namespace Smooth.Operands
{
    public interface IBinarySource<out L, out R> : IBinarySource
    {
        new L LeftOperand { get; }

        new R RightOperand { get; }
    }
}