﻿using Microsoft.Z3;
using Symbolica.Expression;

namespace Symbolica.Computation.Values
{
    internal abstract class Integer : IValue
    {
        protected Integer(Bits size)
        {
            Size = size;
        }

        public Bits Size { get; }

        public abstract BitVecExpr AsBitVector(Context context);
        public abstract BoolExpr AsBool(Context context);

        public FPExpr AsFloat(Context context)
        {
            return context.MkFPToFP(AsBitVector(context), Size.GetSort(context));
        }
    }
}