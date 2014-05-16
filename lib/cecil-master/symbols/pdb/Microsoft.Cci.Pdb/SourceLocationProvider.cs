//-----------------------------------------------------------------------------
//
// Copyright (C) Microsoft Corporation.  All Rights Reserved.
//
//-----------------------------------------------------------------------------

namespace Microsoft.Cci
{
    internal sealed class PdbIteratorScope : ILocalScope
    {
        private readonly uint length;
        private readonly uint offset;

        internal PdbIteratorScope(uint offset, uint length)
        {
            this.offset = offset;
            this.length = length;
        }

        public uint Offset
        {
            get { return offset; }
        }

        public uint Length
        {
            get { return length; }
        }
    }
}