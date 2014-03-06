//
// ArrayType.cs
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
using System.Text;
using Mono.Collections.Generic;
using MD = Mono.Cecil.Metadata;

namespace Mono.Cecil
{
    public struct ArrayDimension
    {
        private int? lower_bound;
        private int? upper_bound;

        public ArrayDimension(int? lowerBound, int? upperBound)
        {
            lower_bound = lowerBound;
            upper_bound = upperBound;
        }

        public int? LowerBound
        {
            get { return lower_bound; }
            set { lower_bound = value; }
        }

        public int? UpperBound
        {
            get { return upper_bound; }
            set { upper_bound = value; }
        }

        public bool IsSized
        {
            get { return lower_bound.HasValue || upper_bound.HasValue; }
        }

        public override string ToString()
        {
            return !IsSized
                       ? string.Empty
                       : lower_bound + "..." + upper_bound;
        }
    }

    public sealed class ArrayType : TypeSpecification
    {
        private Collection<ArrayDimension> dimensions;

        public ArrayType(TypeReference type)
            : base(type)
        {
            Mixin.CheckType(type);
            etype = MD.ElementType.Array;
        }

        public ArrayType(TypeReference type, int rank)
            : this(type)
        {
            Mixin.CheckType(type);

            if (rank == 1)
                return;

            dimensions = new Collection<ArrayDimension>(rank);
            for (int i = 0; i < rank; i++)
                dimensions.Add(new ArrayDimension());
            etype = MD.ElementType.Array;
        }

        public Collection<ArrayDimension> Dimensions
        {
            get
            {
                if (dimensions != null)
                    return dimensions;

                dimensions = new Collection<ArrayDimension>();
                dimensions.Add(new ArrayDimension());
                return dimensions;
            }
        }

        public int Rank
        {
            get { return dimensions == null ? 1 : dimensions.Count; }
        }

        public bool IsVector
        {
            get
            {
                if (dimensions == null)
                    return true;

                if (dimensions.Count > 1)
                    return false;

                ArrayDimension dimension = dimensions[0];

                return !dimension.IsSized;
            }
        }

        public override bool IsValueType
        {
            get { return false; }
            set { throw new InvalidOperationException(); }
        }

        public override string Name
        {
            get { return base.Name + Suffix; }
        }

        public override string FullName
        {
            get { return base.FullName + Suffix; }
        }

        private string Suffix
        {
            get
            {
                if (IsVector)
                    return "[]";

                var suffix = new StringBuilder();
                suffix.Append("[");
                for (int i = 0; i < dimensions.Count; i++)
                {
                    if (i > 0)
                        suffix.Append(",");

                    suffix.Append(dimensions[i].ToString());
                }
                suffix.Append("]");

                return suffix.ToString();
            }
        }

        public override bool IsArray
        {
            get { return true; }
        }
    }
}