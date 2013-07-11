namespace Smooth.ProcessModel
{
    public interface IProcessState
    {
        bool IsCurrent { get; }
        bool IsAbandoned { get; }
        bool IsCompleted { get; }
    }
}