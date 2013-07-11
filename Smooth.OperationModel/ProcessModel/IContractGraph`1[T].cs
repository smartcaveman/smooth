using System.Diagnostics.Contracts;

namespace Smooth.ProcessModel
{
    [ContractClass(typeof(IContractGraphContract<>))]
    public interface IContractGraph<out T>
    {
        [Pure]
        T Invariant { get; }

        [Pure]
        T PreCondition { get; }

        [Pure]
        T PostCondition { get; }
    }

    [Pure, ContractClassFor(typeof(IContractGraph<>))]
    internal abstract class IContractGraphContract<T> : IContractGraph<T>
    {
        [Pure]
        T IContractGraph<T>.Invariant
        {
            get
            {
                Contract.Ensures(!ReferenceEquals(null, Contract.Result<T>()));
                return default(T);
            }
        }

        [Pure]
        T IContractGraph<T>.PreCondition
        {
            get
            {
                Contract.Ensures(!ReferenceEquals(null, Contract.Result<T>()));
                return default(T);
            }
        }

        [Pure]
        T IContractGraph<T>.PostCondition
        {
            get
            {
                Contract.Ensures(!ReferenceEquals(null, Contract.Result<T>()));
                return default(T);
            }
        }
    }
}