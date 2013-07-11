using System;
using Smooth.ProcessModel;

namespace Smooth.Strategies
{
    public interface IProcessStrategy<in TContext> : IStrategy<Action<TContext>>, IContractGraph<Predicate<TContext>>
    {
        ProcessResult Execute(TContext context);
    }
}