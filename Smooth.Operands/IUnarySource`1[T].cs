namespace Smooth.Operands
{
    public interface IUnarySource<out T> : IUnarySource
    {
        new T Operand { get; }
    }
}