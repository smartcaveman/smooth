using System;
using Smooth.OperationModel;

namespace Smooth.Strategies
{
    public interface IActionStrategy<TContext> : IStrategy<Action<TContext>>, IContractGraph<Predicate<TContext>>
    {
        IResult<TContext> Execute(TContext context);
    }
}