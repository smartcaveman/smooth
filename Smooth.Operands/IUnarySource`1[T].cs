namespace Smooth.Operands
{
    public interface IUnarySource<out T> : IUnarySource
    {
        new T Operand { get; }

        IBinarySource<T, R> With<R>(IUnarySource<R> other);
    }
}