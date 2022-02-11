﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Symbolica.Collection;
using Symbolica.Computation.Values.Constants;
using Symbolica.Expression;

namespace Symbolica.Computation;

internal sealed class PersistentSpace<TContext> : IPersistentSpace
    where TContext : IContext, new()
{
    private readonly IPersistentStack<IValue> _assertions;
    private readonly ICollectionFactory _collectionFactory;
    private readonly IModelFactory _modelFactory;
    private readonly ISymbolFactory _symbolFactory;
    private readonly bool _useSymbolicGarbage;

    private PersistentSpace(Bits pointerSize, bool useSymbolicGarbage,
        ISymbolFactory symbolFactory, IModelFactory modelFactory, ICollectionFactory collectionFactory,
        IPersistentStack<IValue> assertions)
    {
        PointerSize = pointerSize;
        _useSymbolicGarbage = useSymbolicGarbage;
        _symbolFactory = symbolFactory;
        _modelFactory = modelFactory;
        _collectionFactory = collectionFactory;
        _assertions = assertions;
    }

    public Bits PointerSize { get; }

    public IPersistentSpace Assert(IValue assertion)
    {
        return new PersistentSpace<TContext>(PointerSize, _useSymbolicGarbage,
            _symbolFactory, _modelFactory, _collectionFactory,
            _assertions.Push(assertion));
    }

    public IModel GetModel(IValue[] constraints)
    {
        return _modelFactory.Create<TContext>(constraints.Concat(_assertions));
    }

    public IExample GetExample()
    {
        return Example.Create(this);
    }

    public IExpression CreateConstant(Bits size, BigInteger value)
    {
        return Expression<TContext>.Create(_collectionFactory,
            ConstantUnsigned.Create(size, value), Enumerable.Empty<Func<IExpression, IExpression>>());
    }

    public IExpression CreateConstantFloat(Bits size, string value)
    {
        return Expression<TContext>.Create(_collectionFactory,
            size.ParseFloat(value), Enumerable.Empty<Func<IExpression, IExpression>>());
    }

    public IExpression CreateGarbage(Bits size)
    {
        return _useSymbolicGarbage
            ? CreateSymbolic(size, null)
            : CreateConstant(size, BigInteger.Zero);
    }

    public IExpression CreateSymbolic(Bits size, string? name)
    {
        return CreateSymbolic(size, name, Enumerable.Empty<Func<IExpression, IExpression>>());
    }

    public IExpression CreateSymbolic(Bits size, string? name, IEnumerable<Func<IExpression, IExpression>> constraints)
    {
        return Expression<TContext>.Create(_collectionFactory,
            Symbol.Create(_symbolFactory, size, name), constraints);
    }

    public static ISpace Create(Bits pointerSize, bool useSymbolicGarbage,
        ISymbolFactory symbolFactory, IModelFactory modelFactory, ICollectionFactory collectionFactory)
    {
        return new PersistentSpace<TContext>(pointerSize, useSymbolicGarbage,
            symbolFactory, modelFactory, collectionFactory,
            collectionFactory.CreatePersistentStack<IValue>());
    }
}
