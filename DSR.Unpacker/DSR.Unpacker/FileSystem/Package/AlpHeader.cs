using System;

namespace DSR.Unpacker
{
    class AlpHeader
    {
        public UInt32 dwMagic { get; set; } //0x24504C41 (ALPx\24)
        public Int32 dwTableOffset { get; set; }
    }
}
