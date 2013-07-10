using System.Collections.Generic;

namespace Smooth.Operands
{
    public interface INarySource<out T> : INarySource
    {
        new IEnumerable<T> Operands { get; }
    }
}