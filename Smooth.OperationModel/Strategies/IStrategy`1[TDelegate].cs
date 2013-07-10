using System;
using System.Diagnostics.Contracts;

namespace Smooth.Strategies
{
    [ContractClass(typeof(IStrategyContract<>))]
    public interface IStrategy<out TDelegate>
        where TDelegate : class
    {
        TDelegate Delegate { get; }
    }

    [ContractClassFor(typeof(IStrategy<>))]
    internal class IStrategyContract<TDelegate> : IStrategy<TDelegate>
        where TDelegate : class
    {
        public TDelegate Delegate
        {
            get
            {
                Contract.Ensures(Contract.Result<TDelegate>() is Delegate);
                return default(TDelegate);
            }
        }
    }
}