using Smooth.Operands;
using Smooth.OperationModel;

namespace Smooth.Strategies
{
    public interface IOperationStrategy<out TDelegate, out TPreCondition, out TPostCondition, in TIn, out TOut> : IOperationStrategy<TDelegate, TPreCondition, TPostCondition, TOut>
        where TDelegate : class /*delegate*/
        where TPreCondition : class /*delegate*/
        where TPostCondition : class /*delegate*/
        where TIn : ISource
    {
        IOperationResult<TOut> Apply(TIn input);
    }
}