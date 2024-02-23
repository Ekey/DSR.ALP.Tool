using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace DSR.Unpacker
{
    class AlpUnpack
    {
        private static List<AlpEntry> m_EntryTable = new List<AlpEntry>();

        public static void iDoIt(String m_AlpFile, String m_DstFolder)
        {
            using (FileStream TAlpStream = File.OpenRead(m_AlpFile))
            {
                var m_Header = new AlpHeader();

                m_Header.dwMagic = TAlpStream.ReadUInt32();
                m_Header.dwTableOffset = TAlpStream.ReadInt32();

                if (m_Header.dwMagic != 0x24504C41)
                {
                    Utils.iSetError("[ERROR]: Invalid magic of ALP file");
                    return;
                }

                TAlpStream.Seek(m_Header.dwTableOffset, SeekOrigin.Begin);

                m_EntryTable.Clear();
                while (true)
                {
                    var m_Entry = new AlpEntry();

                    m_Entry.dwOffset = TAlpStream.ReadInt32();

                    if (m_Entry.dwOffset > 0)
                    {
                        m_Entry.dwSize = TAlpStream.ReadInt32();
                        m_Entry.bNameSize = TAlpStream.ReadByte();
                        m_Entry.m_FileName = Encoding.UTF8.GetString(RC4.iCryptData(TAlpStream.ReadBytes(m_Entry.bNameSize), m_Entry.bNameSize));

                        m_EntryTable.Add(m_Entry);

                        continue;
                    }
                    break;
                }

                foreach (var m_Entry in m_EntryTable)
                {
                    String m_FullPath = m_DstFolder + m_Entry.m_FileName;

                    Utils.iSetInfo("[UNPACKING]: " + m_Entry.m_FileName);
                    Utils.iCreateDirectory(m_FullPath);

                    TAlpStream.Seek(m_Entry.dwOffset, SeekOrigin.Begin);

                    var lpBuffer = TAlpStream.ReadBytes(m_Entry.dwSize);
                    lpBuffer = RC4.iCryptData(lpBuffer, m_Entry.dwSize);

                    File.WriteAllBytes(m_FullPath, lpBuffer);
                }

                TAlpStream.Dispose();
            }
        }
    }
}
