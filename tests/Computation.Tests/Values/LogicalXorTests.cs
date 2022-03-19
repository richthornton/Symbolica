﻿using FluentAssertions;
using Symbolica.Computation.Values.TestData;
using Xunit;

namespace Symbolica.Computation.Values;

public class LogicalXorTests
{
    [Theory]
    [ClassData(typeof(BinaryTestData))]
    private void ShouldCreateEquivalentBitVectors(
        IValue left0, IValue right0,
        IValue left1, IValue right1)
    {
        using var context = PooledContext.Create();

        using var bv0 = LogicalXor.Create(left0, right0).AsBitVector(context);
        using var result0 = bv0.Simplify();

        using var bv1 = LogicalXor.Create(left1, right1).AsBitVector(context);
        using var result1 = bv1.Simplify();

        result0.Should().BeEquivalentTo(result1);
    }

    [Theory]
    [ClassData(typeof(BinaryTestData))]
    private void ShouldCreateEquivalentBooleans(
        IValue left0, IValue right0,
        IValue left1, IValue right1)
    {
        using var context = PooledContext.Create();

        using var b0 = LogicalXor.Create(left0, right0).AsBool(context);
        using var result0 = b0.Simplify();

        using var b1 = LogicalXor.Create(left1, right1).AsBool(context);
        using var result1 = b1.Simplify();

        result0.Should().BeEquivalentTo(result1);
    }
}
