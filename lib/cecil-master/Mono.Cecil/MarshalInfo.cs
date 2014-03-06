//
// MarshalInfo.cs
//
// Author:
//   Jb Evain (jbevain@gmail.com)
//
// Copyright (c) 2008 - 2011 Jb Evain
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;

namespace Mono.Cecil
{
    public class MarshalInfo
    {
        internal NativeType native;

        public MarshalInfo(NativeType native)
        {
            this.native = native;
        }

        public NativeType NativeType
        {
            get { return native; }
            set { native = value; }
        }
    }

    public sealed class ArrayMarshalInfo : MarshalInfo
    {
        internal NativeType element_type;
        internal int size;
        internal int size_parameter_index;
        internal int size_parameter_multiplier;

        public ArrayMarshalInfo()
            : base(NativeType.Array)
        {
            element_type = NativeType.None;
            size_parameter_index = -1;
            size = -1;
            size_parameter_multiplier = -1;
        }

        public NativeType ElementType
        {
            get { return element_type; }
            set { element_type = value; }
        }

        public int SizeParameterIndex
        {
            get { return size_parameter_index; }
            set { size_parameter_index = value; }
        }

        public int Size
        {
            get { return size; }
            set { size = value; }
        }

        public int SizeParameterMultiplier
        {
            get { return size_parameter_multiplier; }
            set { size_parameter_multiplier = value; }
        }
    }

    public sealed class CustomMarshalInfo : MarshalInfo
    {
        internal string cookie;
        internal Guid guid;
        internal TypeReference managed_type;
        internal string unmanaged_type;

        public CustomMarshalInfo()
            : base(NativeType.CustomMarshaler)
        {
        }

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }

        public string UnmanagedType
        {
            get { return unmanaged_type; }
            set { unmanaged_type = value; }
        }

        public TypeReference ManagedType
        {
            get { return managed_type; }
            set { managed_type = value; }
        }

        public string Cookie
        {
            get { return cookie; }
            set { cookie = value; }
        }
    }

    public sealed class SafeArrayMarshalInfo : MarshalInfo
    {
        internal VariantType element_type;

        public SafeArrayMarshalInfo()
            : base(NativeType.SafeArray)
        {
            element_type = VariantType.None;
        }

        public VariantType ElementType
        {
            get { return element_type; }
            set { element_type = value; }
        }
    }

    public sealed class FixedArrayMarshalInfo : MarshalInfo
    {
        internal NativeType element_type;
        internal int size;

        public FixedArrayMarshalInfo()
            : base(NativeType.FixedArray)
        {
            element_type = NativeType.None;
        }

        public NativeType ElementType
        {
            get { return element_type; }
            set { element_type = value; }
        }

        public int Size
        {
            get { return size; }
            set { size = value; }
        }
    }

    public sealed class FixedSysStringMarshalInfo : MarshalInfo
    {
        internal int size;

        public FixedSysStringMarshalInfo()
            : base(NativeType.FixedSysString)
        {
            size = -1;
        }

        public int Size
        {
            get { return size; }
            set { size = value; }
        }
    }
}