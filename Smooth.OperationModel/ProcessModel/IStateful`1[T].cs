namespace Smooth.ProcessModel
{
    public interface IStateful<out T>
    {
        T State { get; }
    }
}