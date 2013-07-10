namespace Smooth.OperationModel
{
    public interface IContractGraph<out T>
    {
        T Invariant { get; }

        T PreCondition { get; }

        T PostCondition { get; }
    }
}