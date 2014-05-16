//-----------------------------------------------------------------------------
//
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the Microsoft Public License.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//-----------------------------------------------------------------------------

namespace Microsoft.Cci.Pdb
{
    internal class PdbSlot
    {
        internal ushort flags;
        internal string name;
        internal uint slot;
        internal uint typeToken;
        //internal uint segment;
        //internal uint address;

        internal PdbSlot(BitAccess bits)
        {
            AttrSlotSym slot;

            bits.ReadUInt32(out slot.index);
            bits.ReadUInt32(out slot.typind);
            bits.ReadUInt32(out slot.offCod);
            bits.ReadUInt16(out slot.segCod);
            bits.ReadUInt16(out slot.flags);
            bits.ReadCString(out slot.name);

            this.slot = slot.index;
            typeToken = slot.typind;
            name = slot.name;
            flags = slot.flags;
            //this.segment = slot.segCod;
            //this.address = slot.offCod;
        }
    }
}