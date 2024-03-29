﻿/*
 MIT License

Copyright (c) 2021 ikorin24

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

 */

#nullable enable

#if NET5_0 || NETCOREAPP3_1
#define FAST_SPAN
#endif

using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace UnmanageUtility
{
    /// <summary>
    /// Array class which is allocated in unmanaged memory.<para/>
    /// Only for unmanaged types. (e.g. int, float, recursive-unmanaged struct, and so on.)
    /// </summary>
    /// <typeparam name="T">type of array</typeparam>
    [DebuggerTypeProxy(typeof(UnmanagedArrayDebuggerTypeProxy<>))]
    [DebuggerDisplay("UnmanagedArray<{typeof(T).Name}>[{_length}]")]
    public sealed class UnmanagedArray<T> : IList<T>, IReadOnlyList<T>, IList, IReadOnlyCollection<T>, IDisposable
        where T : unmanaged
    {
        private static UnmanagedArray<T> _empty = new UnmanagedArray<T>(0);

        [ThreadStatic]
        internal static UnmanagedList<T>? _helperList;


        internal int _length;
        internal IntPtr _array;

        /// <summary>Get empty array</summary>
        public static UnmanagedArray<T> Empty => _empty;

        /// <summary>Get pointer address of this array.</summary>
        public IntPtr Ptr => _array;

        /// <summary>Get <see cref="UnmanagedArray{T}"/> is disposed.</summary>
        public bool IsDisposed => _array == IntPtr.Zero;

        /// <summary>Get the specific item of specific index.</summary>
        /// <param name="i">index</param>
        /// <returns>The item of specific index</returns>
        public T this[int i]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => GetReference(i);
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => GetReference(i) = value;
        }

        /// <summary>Get length of this array</summary>
        public int Length => _length;   // No checking disposed because this property is safe. (for performance of "for(int i = 0; i < unmanagedArray.Length; i++)")


        // *** NOTICE ***
        // T[] a = new T[10];
        // (a as ICollection<T>).IsReadOnly   ----> true
        // (a as IList).IsReadOnly   ----> false
        // 
        // ↓ I copied thier values of the properties.

        bool ICollection<T>.IsReadOnly => true;

        bool IList.IsReadOnly => false;

        bool IList.IsFixedSize => true;

        int ICollection<T>.Count => _length;   // No checking disposed because this property is safe.

        int IReadOnlyCollection<T>.Count => _length;   // No checking disposed because this property is safe.

        int ICollection.Count => _length;   // No checking disposed because this property is safe.

        object ICollection.SyncRoot => this;

        bool ICollection.IsSynchronized => false;

#pragma warning disable CS8769
        object IList.this[int index] { get => this[index]; set => this[index] = (T)value; }
#pragma warning restore CS8769

        /// <summary>UnmanagedArray Constructor</summary>
        /// <param name="length">Length of array</param>
        public unsafe UnmanagedArray(int length) : this(length, default) { }

        /// <summary>Create new <see cref="UnmanagedArray{T}"/> filled by specified element.</summary>
        /// <param name="length">length of array</param>
        /// <param name="fill">element that fills array</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe UnmanagedArray(int length, T fill)
        {
            if(length < 0) {
                ThrowOutOfRange();
                static void ThrowOutOfRange() => throw new ArgumentOutOfRangeException();
            }
            var bytes = sizeof(T) * length;
            if(bytes == 0) { return; }
            _array = Marshal.AllocHGlobal(bytes);
            _length = length;
            new Span<T>((void*)_array, _length).Fill(fill);
        }

        private unsafe UnmanagedArray()
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal unsafe static UnmanagedArray<T> CreateWithoutZeroFill(int length)
        {
            if(length < 0) {
                ThrowOutOfRange();
                static void ThrowOutOfRange() => throw new ArgumentOutOfRangeException(nameof(length));
            }
            var umarray = new UnmanagedArray<T>();
            var bytes = length * sizeof(T);
            if(bytes > 0) {
                umarray._array = Marshal.AllocHGlobal(bytes);
                umarray._length = length;
            }
            return umarray;
        }

        /// <summary>Create new <see cref="UnmanagedArray{T}"/>, those elements are copied from <see cref="ReadOnlySpan{T}"/>.</summary>
        /// <param name="span">Elements of the <see cref="UnmanagedArray{T}"/> are initialized by this <see cref="ReadOnlySpan{T}"/>.</param>
        public unsafe UnmanagedArray(ReadOnlySpan<T> span)
        {
            var bytes = span.Length * sizeof(T);
            if(bytes == 0) { return; }
            _array = Marshal.AllocHGlobal(bytes);
            _length = span.Length;
            span.CopyTo(new Span<T>((void*)_array, _length));
        }

        /// <summary>Finalizer of <see cref="UnmanagedArray{T}"/></summary>
        ~UnmanagedArray() => Dispose(false);

        /// <summary>Get reference to head item (Returns ref to null if empty)</summary>
        /// <returns>reference to head item</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe ref T GetReference()
        { 
#if DEBUG
            if(_length == 0) {
                Debug.Assert(_array == IntPtr.Zero);
            }
#endif
            return ref Unsafe.AsRef<T>((T*)_array);
        }

        /// <summary>Get reference to head item (Returns ref to null if empty)</summary>
        /// <returns>reference to head item</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe ref T GetReference(int index)
        {
            if((uint)index >= (uint)_length) {
                ThrowOutOfRange();
                static void ThrowOutOfRange() => throw new IndexOutOfRangeException();
            }
            return ref Unsafe.Add(ref GetReference(), index);
        }

        /// <summary>Get enumerator instance.</summary>
        /// <returns></returns>
        public Enumerator GetEnumerator()
        {
            ThrowIfDisposed();
            return new Enumerator(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            ThrowIfDisposed();
            // Avoid boxing by using class enumerator.
            return new EnumeratorClass(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            ThrowIfDisposed();
            // Avoid boxing by using class enumerator.
            return new EnumeratorClass(this);
        }

        /// <summary>Get index of the item</summary>
        /// <param name="item">target item</param>
        /// <returns>index (if not contain, value is -1)</returns>
        public unsafe int IndexOf(T item)
        {
            ThrowIfDisposed();
            for(int i = 0; i < _length; i++) {
                if(EqualityComparer<T>.Default.Equals(((T*)_array)[i], item)) {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>Get whether this instance contains the item.</summary>
        /// <param name="item">target item</param>
        /// <returns>true: This array contains the target item. false: not contain</returns>
        public unsafe bool Contains(T item)
        {
            ThrowIfDisposed();
            for(int i = 0; i < _length; i++) {
                if(EqualityComparer<T>.Default.Equals(((T*)_array)[i], item)) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>Copy to managed memory</summary>
        /// <param name="array">managed memory array</param>
        /// <param name="arrayIndex">start index of destination array</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            ThrowIfDisposed();
            if(array == null) { throw new ArgumentNullException(nameof(array)); }
            if((uint)arrayIndex >= (uint)array.Length) { throw new ArgumentOutOfRangeException(nameof(arrayIndex)); }
            if(arrayIndex + _length > array.Length) { throw new ArgumentException("There is not enouph length of destination array"); }
            unsafe {
                var objsize = sizeof(T);
                fixed(T* arrayPtr = array) {
                    var byteLen = (long)(_length * objsize);
                    var dest = new IntPtr(arrayPtr) + arrayIndex * objsize;
                    Buffer.MemoryCopy((void*)_array, (void*)dest, byteLen, byteLen);
                }
            }
        }

        void IList<T>.Insert(int index, T item) => throw new NotSupportedException();
        void IList<T>.RemoveAt(int index) => throw new NotSupportedException();
        void ICollection<T>.Add(T item) => throw new NotSupportedException();
        bool ICollection<T>.Remove(T item) => throw new NotSupportedException();
        void ICollection<T>.Clear() => throw new NotSupportedException();

        /// <summary>Get pointer address of specified index.</summary>
        /// <param name="index">index</param>
        /// <returns>pointer address</returns>
        [Obsolete("Use instead 'Ptr + sizeof(T) * index', that is faster.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe IntPtr GetPtrIndexOf(int index)
        {
            ThrowIfDisposed();
            return Ptr + sizeof(T) + index;
            
            if((uint)index >= (uint)_length) {
                ThrowOutOfRange();
                static void ThrowOutOfRange() => throw new IndexOutOfRangeException();
            }
            return new IntPtr((T*)_array + index);
        }

        /// <summary>Copy fron <see cref="UnmanagedArray{T}"/>.</summary>
        /// <param name="array">source array of type <see cref="UnmanagedArray{T}"/></param>
        public void CopyFrom(UnmanagedArray<T> array) => CopyFrom(array.Ptr, 0, array.Length);

        /// <summary>Copy from <see cref="ReadOnlySpan{T}"/> to this <see cref="UnmanagedArray{T}"/> of index 0.</summary>
        /// <param name="span"><see cref="ReadOnlySpan{T}"/> object.</param>
        public void CopyFrom(ReadOnlySpan<T> span) => CopyFrom(span, 0);

        /// <summary>Copy from <see cref="ReadOnlySpan{T}"/> to this <see cref="UnmanagedArray{T}"/> of specified index.</summary>
        /// <param name="source"><see cref="ReadOnlySpan{T}"/> object.</param>
        /// <param name="start">start index of destination. (destination is this <see cref="UnmanagedArray{T}"/>.)</param>
        public unsafe void CopyFrom(ReadOnlySpan<T> source, int start)
        {
            ThrowIfDisposed();
            if(start < 0) { throw new ArgumentOutOfRangeException(); }
            if(start + source.Length > _length) { throw new ArgumentOutOfRangeException(); }
            var objsize = sizeof(T);
            fixed(T* ptr = source) {
                var byteLen = (long)(source.Length * objsize);
                Buffer.MemoryCopy(ptr, (void*)(_array + start * objsize), byteLen, byteLen);
            }
        }

        /// <summary>Copy from unmanaged.</summary>
        /// <param name="source">unmanaged source pointer</param>
        /// <param name="start">start index of destination. (destination is this <see cref="UnmanagedArray{T}"/>.)</param>
        /// <param name="length">count of copied item. (NOT length of bytes.)</param>
        public unsafe void CopyFrom(IntPtr source, int start, int length)
        {
            ThrowIfDisposed();
            if(length == 0) { return; }
            if(source == IntPtr.Zero) { throw new ArgumentNullException("source is null"); }
            if(start < 0 || length < 0) { throw new ArgumentOutOfRangeException(); }
            if(start + length > _length) { throw new ArgumentOutOfRangeException(); }
            var objsize = sizeof(T);
            var byteLen = (long)(length * objsize);
            Buffer.MemoryCopy((void*)source, (void*)(_array + start * objsize), byteLen, byteLen);
        }

        /// <summary>Return <see cref="Span{T}"/> of this <see cref="UnmanagedArray{T}"/>.</summary>
        /// <returns><see cref="Span{T}"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe Span<T> AsSpan()
        {
#if DEBUG
            if(_length == 0) {
                Debug.Assert(_array == IntPtr.Zero);
            }
#endif
#if FAST_SPAN
            return MemoryMarshal.CreateSpan(ref Unsafe.AsRef<T>((T*)_array), _length);
#else
            return new Span<T>((T*)_array, _length);
#endif
        }

        /// <summary>Return <see cref="Span{T}"/> starts with specified index.</summary>
        /// <param name="start">start index</param>
        /// <returns><see cref="Span{T}"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe Span<T> AsSpan(int start)
        {
#if DEBUG
            if(_length == 0) {
                Debug.Assert(_array == IntPtr.Zero);
            }
#endif
            if((uint)start > (uint)_length) {
                ThrowOutOfRange();
                static void ThrowOutOfRange() => throw new ArgumentOutOfRangeException(nameof(start));
            }
#if FAST_SPAN
            return MemoryMarshal.CreateSpan(ref Unsafe.AsRef<T>((T*)_array + start), _length - start);
#else
            return new Span<T>((T*)_array + start, _length - start);
#endif
        }

        /// <summary>Return <see cref="Span{T}"/> of specified length starts with specified index.</summary>
        /// <param name="start">start index</param>
        /// <param name="length">length of span</param>
        /// <returns><see cref="Span{T}"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe Span<T> AsSpan(int start, int length)
        {
#if DEBUG
            if(_length == 0) {
                Debug.Assert(_array == IntPtr.Zero);
            }
#endif
            if((uint)start > (uint)_length) {
                ThrowOutOfRange();
                static void ThrowOutOfRange() => throw new ArgumentOutOfRangeException(nameof(start));
            }
            if((uint)length > (uint)_length - (uint)start) {
                ThrowOutOfRange();
                static void ThrowOutOfRange() => throw new ArgumentOutOfRangeException(nameof(length));
            }
#if FAST_SPAN
            return MemoryMarshal.CreateSpan(ref Unsafe.AsRef<T>((T*)_array + start), length);
#else
            return new Span<T>((T*)_array + start, length);
#endif
        }

        /// <summary>Create new <see cref="UnmanagedArray{T}"/> whose values are initialized by memory layout of specified structure.</summary>
        /// <typeparam name="TStruct">type of source structure</typeparam>
        /// <param name="obj">source structure</param>
        /// <returns>instance of <see cref="UnmanagedArray{T}"/> whose values are initialized by <paramref name="obj"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe UnmanagedArray<T> CreateFromStruct<TStruct>(ref TStruct obj) where TStruct : unmanaged
        {
            var structSize = sizeof(TStruct);
            var itemSize = sizeof(T);
            var arrayLen = structSize / itemSize + (structSize % itemSize > 0 ? 1 : 0);
            var array = new UnmanagedArray<T>(arrayLen);
            fixed(TStruct* ptr = &obj) {
                Buffer.MemoryCopy(ptr, (void*)array._array, structSize, structSize);
            }
            return array;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe UnmanagedArray<T> DirectCreateWithoutCopy(T* ptr, int length)
        {
            // Be Careful !!!! This method is very unsafe !!
            // 'ptr' must be pointer to unmanaged heap memory.

            var array = new UnmanagedArray<T>();
            array._array = (IntPtr)ptr;
            array._length = length;
            return array;
        }

        /// <summary>
        /// Dispose this instance and release unmanaged memory.<para/>
        /// If already disposed, do nothing.<para/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe void Dispose(bool disposing)
        {
            if(_array == IntPtr.Zero) { return; }
            Marshal.FreeHGlobal(_array);
            Debug.Assert(sizeof(T) * _length > 0);
            _array = IntPtr.Zero;
            _length = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ThrowIfDisposed()
        {
            if(IsDisposed) { throw new ObjectDisposedException(nameof(UnmanagedArray<T>), "Memory of array is already free."); }
        }

#pragma warning disable CS8769
        int IList.Add(object value) => throw new NotSupportedException();
        void IList.Clear() => throw new NotSupportedException();
        void IList.Insert(int index, object value) => throw new NotSupportedException();
        void IList.Remove(object value) => throw new NotSupportedException();
        void IList.RemoveAt(int index) => throw new NotSupportedException();
        bool IList.Contains(object value) => (value is T v) ? Contains(v) : false;
        int IList.IndexOf(object value) => (value is T v) ? IndexOf(v) : -1;
        void ICollection.CopyTo(Array array, int index) => CopyTo((T[])array, index);
#pragma warning restore CS8769

        /// <summary>Enumerator of <see cref="UnmanagedArray{T}"/></summary>
        public unsafe struct Enumerator : IEnumerator<T>, IEnumerator
        {
            private readonly T* _ptr;
            private readonly int _len;
            private int _index;

            /// <summary>Get current element</summary>
            public T Current { get; private set; }

            internal Enumerator(UnmanagedArray<T> array)
            {
                _ptr = (T*)array._array;
                _len = array._length;
                _index = 0;
                Current = default;
            }

            /// <summary>Dispose of <see cref="IDisposable"/></summary>
            public void Dispose() { }

            /// <summary>Move to next element</summary>
            /// <returns>true if success to move next. false to end.</returns>
            public bool MoveNext()
            {
                if((uint)_index < (uint)_len) {
                    Current = _ptr[_index];
                    _index++;
                    return true;
                }
                return MoveNextRare();
            }

            private bool MoveNextRare()
            {
                _index = _len + 1;
                Current = default;
                return false;
            }

            object IEnumerator.Current => Current;

            void IEnumerator.Reset()
            {
                _index = 0;
                Current = default;
            }
        }

        /// <summary>Enumerator of <see cref="UnmanagedArray{T}"/></summary>
        public unsafe class EnumeratorClass : IEnumerator<T>, IEnumerator
        {
            private readonly T* _ptr;
            private readonly int _len;
            private int _index;

            /// <summary>Get current element</summary>
            public T Current { get; private set; }

            internal EnumeratorClass(UnmanagedArray<T> array)
            {
                _ptr = (T*)array._array;
                _len = array._length;
                _index = 0;
                Current = default;
            }

            /// <summary>Dispose of <see cref="IDisposable"/></summary>
            public void Dispose() { }

            /// <summary>Move to next element</summary>
            /// <returns>true if success to move next. false to end.</returns>
            public bool MoveNext()
            {
                if((uint)_index < (uint)_len) {
                    Current = _ptr[_index];
                    _index++;
                    return true;
                }
                return MoveNextRare();
            }

            private bool MoveNextRare()
            {
                _index = _len + 1;
                Current = default;
                return false;
            }

            object IEnumerator.Current => Current;

            void IEnumerator.Reset()
            {
                _index = 0;
                Current = default;
            }
        }
    }

    /// <summary>Define extension methods of <see cref="UnmanagedArray{T}"/></summary>
    public static class UnmanagedArrayExtension
    {
        /// <summary>Create a new instance of <see cref="UnmanagedArray{T}"/> initialized by source.</summary>
        /// <typeparam name="T">Type of item in array</typeparam>
        /// <param name="source">source which initializes new array.</param>
        /// <returns>instance of <see cref="UnmanagedArray{T}"/></returns>
        public static unsafe UnmanagedArray<T> ToUnmanagedArray<T>(this IEnumerable<T> source) where T : unmanaged
        {
            if(source == null) { throw new ArgumentNullException(nameof(source)); }
            if(source is T[] managedArray) {
                var array = UnmanagedArray<T>.CreateWithoutZeroFill(managedArray.Length);
                managedArray.AsSpan().CopyTo(array.AsSpan());
                return array;
            }
            else {
                var helper = (UnmanagedArray<T>._helperList ??= new UnmanagedList<T>());
                helper.AddRange(source);
                helper.TransferInnerMemoryOwnership(out var ptr, out _, out var length);

                // Capacity of allocated memory may be larger than length to use,
                // but this occurs no problem.

                return UnmanagedArray<T>.DirectCreateWithoutCopy((T*)ptr, length);
            }
        }

        /// <summary>Create a new instance of <see cref="UnmanagedArray{T}"/> initialized by source.</summary>
        /// <typeparam name="T">Type of item in array</typeparam>
        /// <param name="source">source which initializes new array.</param>
        /// <returns>instance of <see cref="UnmanagedArray{T}"/></returns>
        public static UnmanagedArray<T> ToUnmanagedArray<T>(this ReadOnlySpan<T> source) where T : unmanaged
        {
            return new UnmanagedArray<T>(source);
        }

        /// <summary>Create a new instance of <see cref="UnmanagedArray{T}"/> initialized by source.</summary>
        /// <typeparam name="T">Type of item in array</typeparam>
        /// <param name="source">source which initializes new array.</param>
        /// <returns>instance of <see cref="UnmanagedArray{T}"/></returns>
        public static UnmanagedArray<T> ToUnmanagedArray<T>(this Span<T> source) where T : unmanaged
        {
            return new UnmanagedArray<T>(source);
        }

        /// <summary>Create a new instance of <see cref="UnmanagedArray{T}"/> initialized by source.</summary>
        /// <typeparam name="T">Type of item in array</typeparam>
        /// <param name="source">source which initializes new array.</param>
        /// <returns>instance of <see cref="UnmanagedArray{T}"/></returns>
        public static UnmanagedArray<T> ToUnmanagedArray<T>(this ReadOnlyMemory<T> source) where T : unmanaged
        {
            return new UnmanagedArray<T>(source.Span);
        }

        /// <summary>Create a new instance of <see cref="UnmanagedArray{T}"/> initialized by source.</summary>
        /// <typeparam name="T">Type of item in array</typeparam>
        /// <param name="source">source which initializes new array.</param>
        /// <returns>instance of <see cref="UnmanagedArray{T}"/></returns>
        public static UnmanagedArray<T> ToUnmanagedArray<T>(this Memory<T> source) where T : unmanaged
        {
            return new UnmanagedArray<T>(source.Span);
        }
    }

    internal class UnmanagedArrayDebuggerTypeProxy<T> where T : unmanaged
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly UnmanagedArray<T> _entity;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items
        {
            get
            {
                var items = new T[_entity.Length];
                _entity.CopyTo(items, 0);
                return items;
            }
        }

        public UnmanagedArrayDebuggerTypeProxy(UnmanagedArray<T> entity) => _entity = entity;
    }
}
