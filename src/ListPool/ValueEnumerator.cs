﻿using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ListPool
{
    public struct ValueEnumerator<TSource> : IEnumerator<TSource>
    {
        private readonly TSource[] _source;
        private readonly int _itemsCount;
        private int _index;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValueEnumerator(TSource[] source, int itemsCount)
        {
            _source = source;
            _itemsCount = itemsCount;
            _index = -1;
        }

        [MaybeNull]
        public readonly ref TSource Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref _source[_index];
        }

        [MaybeNull]
        readonly TSource IEnumerator<TSource>.Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return _source[_index]; }
        }

        [MaybeNull]
        readonly object? IEnumerator.Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return _source[_index]; }
        } 

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext() => ++_index < _itemsCount;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            _index = -1;
        }

        public readonly void Dispose()
        {
        }
    }
}
