﻿using System.Collections.Generic;
using System.Linq;
using Symbolica.Computation.Values.Constants;
using Symbolica.Expression;
using Xunit;

namespace Symbolica.Computation.Values.TestData;

internal sealed class BinaryTestData : TheoryData<
    IConstantValue, IConstantValue,
    SymbolicUnsigned, SymbolicUnsigned>
{
    public BinaryTestData()
    {
        foreach (var left in Values())
        foreach (var right in Values())
            Add(left, right,
                new SymbolicUnsigned(left), new SymbolicUnsigned(right));
    }

    private static IEnumerable<ConstantUnsigned> Values()
    {
        return Enumerable.Range(-8, 24).Select(v => ConstantUnsigned.Create((Bits) 4U, v));
    }
}