﻿using FluentAssertions;
using Symbolica.Computation.Values.TestData;
using Symbolica.Expression;
using Xunit;

namespace Symbolica.Computation.Values;

public class TruncateTests
{
    private static readonly DisposableContext Context = new();

    [Theory]
    [ClassData(typeof(TruncateTestData))]
    private void ShouldCreateEquivalentConstants(Bits size,
        IConstantValue constantValue,
        SymbolicUnsigned symbolicValue)
    {
        var constant = Truncate.Create(size, constantValue).AsConstant(Context);
        var symbolic = Truncate.Create(size, symbolicValue).AsConstant(Context);

        constant.Should().Be(symbolic);
    }

    [Theory]
    [ClassData(typeof(TruncateTestData))]
    private void ShouldCreateEquivalentBitVectors(Bits size,
        IConstantValue constantValue,
        SymbolicUnsigned symbolicValue)
    {
        var constant = Truncate.Create(size, constantValue).AsBitVector(Context).Simplify();
        var symbolic = Truncate.Create(size, symbolicValue).AsBitVector(Context).Simplify();

        constant.Should().BeEquivalentTo(symbolic);
    }

    [Theory]
    [ClassData(typeof(TruncateTestData))]
    private void ShouldCreateEquivalentBooleans(Bits size,
        IConstantValue constantValue,
        SymbolicUnsigned symbolicValue)
    {
        var constant = Truncate.Create(size, constantValue).AsBool(Context).Simplify();
        var symbolic = Truncate.Create(size, symbolicValue).AsBool(Context).Simplify();

        constant.Should().BeEquivalentTo(symbolic);
    }
}
