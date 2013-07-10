using System.Collections;

namespace Smooth.Operands
{
    public interface INarySource : ISource
    {
        IEnumerable Operands { get; }

        int Arity { get; }
    }
}