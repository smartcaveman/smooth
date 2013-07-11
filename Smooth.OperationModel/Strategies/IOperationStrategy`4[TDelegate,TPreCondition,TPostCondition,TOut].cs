using System;
using System.Diagnostics.Contracts;
using Smooth.OperationModel;

namespace Smooth.Strategies
{
    [ContractClass(typeof(IOperationStrategyContract<,,,>))]
    public interface IOperationStrategy<out TDelegate, out TPreCondition, out TPostCondition, out TOut> : IStrategy<TDelegate>
        where TDelegate : class /*delegate*/
        where TPreCondition : class /*delegate*/
        where TPostCondition : class /*delegate*/
    {
        TPreCondition PreCondition { get; }

        TPostCondition PostCondition { get; }

        IOperationResult<TOut> Apply(object[] input);
    }

    [ContractClassFor(typeof(IOperationStrategy<,,,>))]
    internal abstract class IOperationStrategyContract<TDelegate, TPreCondition, TPostCondition, TOut> : IOperationStrategy<TDelegate, TPreCondition, TPostCondition, TOut>
        where TDelegate : class
        where TPreCondition : class
        where TPostCondition : class
    {
        TDelegate IStrategy<TDelegate>.Delegate
        {
            get { throw new System.NotImplementedException(); }
        }

        TPreCondition IOperationStrategy<TDelegate, TPreCondition, TPostCondition, TOut>.PreCondition
        {
            get
            {
                Contract.Ensures(Contract.Result<TPreCondition>() is Delegate);
                return default(TPreCondition);
            }
        }

        TPostCondition IOperationStrategy<TDelegate, TPreCondition, TPostCondition, TOut>.PostCondition
        {
            get
            {
                Contract.Ensures(Contract.Result<TPostCondition>() is Delegate);
                return default(TPostCondition);
            }
        }

        IOperationResult<TOut> IOperationStrategy<TDelegate, TPreCondition, TPostCondition, TOut>.Apply(object[] input)
        {
            Contract.Ensures(Contract.Result<IOperationResult<TOut>>() != null);
            return default(IOperationResult<TOut>);
        }
    }
}