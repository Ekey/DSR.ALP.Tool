using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace DSR.Packer
{
    class AlpPack
    {
        private static List<AlpEntry> m_EntryTable = new List<AlpEntry>();

        public static void iDoIt(String m_SrcFolder, String m_DstFile)
        {
            var m_Files = Directory.GetFiles(m_SrcFolder, "*.*", SearchOption.AllDirectories);

            using (BinaryWriter TAlpStream = new BinaryWriter(File.Open(m_DstFile, FileMode.Create)))
            {
                var m_Header = new AlpHeader();

                m_Header.dwMagic = 0x24504C41;
                m_Header.dwTableOffset = 0;

                TAlpStream.Write(m_Header.dwMagic);
                TAlpStream.Write(m_Header.dwTableOffset);

                m_EntryTable.Clear();
                foreach (var m_File in m_Files)
                {
                    var m_Entry = new AlpEntry();

                    var lpBuffer = File.ReadAllBytes(m_File);
                    lpBuffer = RC4.iCryptData(lpBuffer, lpBuffer.Length);

                    m_Entry.dwOffset = (Int32)TAlpStream.BaseStream.Position;
                    m_Entry.dwSize = lpBuffer.Length;
                    m_Entry.m_FileName = Path.GetFileName(m_File);
                    m_Entry.bNameSize = m_Entry.m_FileName.Length;

                    m_EntryTable.Add(m_Entry);

                    TAlpStream.Write(lpBuffer);
                }

                m_Header.dwTableOffset = (Int32)TAlpStream.BaseStream.Position;

                foreach (var m_Entry in m_EntryTable)
                {
                    TAlpStream.Write(m_Entry.dwOffset);
                    TAlpStream.Write(m_Entry.dwSize);
                    TAlpStream.Write((Byte)m_Entry.bNameSize);

                    var lpFileName = Encoding.UTF8.GetBytes(m_Entry.m_FileName);
                    lpFileName = RC4.iCryptData(lpFileName, lpFileName.Length);
                    TAlpStream.Write(lpFileName);
                }

                TAlpStream.Write(0);
                TAlpStream.Seek(4, SeekOrigin.Begin);
                TAlpStream.Write(m_Header.dwTableOffset);

                TAlpStream.Dispose();
            }
        }
    }
}
