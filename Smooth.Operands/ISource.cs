using System;

namespace Smooth.Operands
{
    public interface ISource : IEquatable<INarySource>
    {
        bool IsInitial { get; }
    }
}