﻿namespace Smooth.Operands
{
    public interface IUnarySource : ISource
    {
        object Operand { get; }
    }
}