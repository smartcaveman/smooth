using System;
using Smooth.Operands;

namespace Smooth.OperationModel
{
    public interface IResult<out TOut> : IResult
    {
        new TOut Output { get; }

        IResult<TNext> Select<TNext>(Func<TOut, TNext> f);

        new IUnarySource<TOut> ToSource();
    }
}