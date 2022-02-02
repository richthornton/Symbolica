﻿using System;
using Microsoft.Z3;
using Symbolica.Computation.Values.Constants;

namespace Symbolica.Computation.Values;

internal sealed class FloatFloor : Float
{
    private readonly IValue _value;

    private FloatFloor(IValue value)
        : base(value.Size)
    {
        _value = value;
    }

    public override FPExpr AsFloat(Context context)
    {
        return context.MkFPRoundToIntegral(context.MkFPRTN(), _value.AsFloat(context));
    }

    public static IValue Create(IValue value)
    {
        return Value.Create(value,
            v => new ConstantSingle(MathF.Floor(v)),
            v => new ConstantDouble(Math.Floor(v)),
            v => new FloatFloor(v));
    }
}