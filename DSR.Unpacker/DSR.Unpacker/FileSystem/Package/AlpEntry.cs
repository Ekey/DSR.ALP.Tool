using System;

namespace DSR.Unpacker
{
    class AlpEntry
    {
        public Int32 dwOffset { get; set; }
        public Int32 dwSize { get; set; }
        public Int32 bNameSize { get; set; }
        public String m_FileName { get; set; }
    }
}
