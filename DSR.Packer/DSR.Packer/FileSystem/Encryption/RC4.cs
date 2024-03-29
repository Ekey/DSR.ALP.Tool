﻿using System;

namespace DSR.Packer
{
    class RC4
    {
        private static readonly Byte[] lpKey = new Byte[256]
        {
            0x20, 0xD5, 0xDE, 0x6D, 0xF8, 0xF6, 0x2A, 0x1F, 0x09, 0xA4, 0xE0, 0xDB, 0x63, 0x0F, 0x8A, 0xA9,
            0xDF, 0xD9, 0x05, 0xAB, 0x76, 0x4F, 0x69, 0x9E, 0x2F, 0x28, 0xB0, 0xFC, 0x5D, 0x2A, 0x35, 0xC8,
            0x07, 0x56, 0xE9, 0x7A, 0x8C, 0xEA, 0x38, 0xE8, 0x8B, 0x07, 0x32, 0xA2, 0x79, 0x3C, 0x81, 0xFC,
            0x76, 0xDA, 0xA6, 0xC2, 0xFC, 0xA2, 0x9D, 0x86, 0xCB, 0xE2, 0xC2, 0x92, 0xB4, 0x96, 0x7E, 0x30,
            0x74, 0x14, 0x27, 0xEB, 0xBC, 0xDA, 0x86, 0x92, 0xDC, 0xF9, 0x08, 0xC1, 0x2A, 0x8E, 0x3D, 0x7F,
            0xCD, 0x26, 0x3C, 0xCC, 0x85, 0x8E, 0x73, 0x6F, 0xC0, 0x2D, 0xE1, 0x3E, 0x0C, 0xC9, 0x17, 0x3F,
            0x2E, 0x7D, 0xFA, 0x65, 0x49, 0xCF, 0xB3, 0xEF, 0xC0, 0x91, 0x7E, 0x84, 0x89, 0xA1, 0x14, 0x05,
            0x42, 0x46, 0x63, 0x7F, 0x66, 0x10, 0xAA, 0x1B, 0xA8, 0x4E, 0xD6, 0x4F, 0xBE, 0x19, 0x54, 0x45,
            0x2E, 0xD2, 0xF4, 0x62, 0x44, 0x31, 0x78, 0x0E, 0x1B, 0x27, 0x23, 0x17, 0xB6, 0x45, 0xB9, 0x49,
            0x6E, 0x32, 0xCB, 0xB2, 0x78, 0x6B, 0x51, 0x33, 0xDE, 0x1C, 0x42, 0x6F, 0xC2, 0x44, 0xC6, 0x10,
            0x7E, 0x16, 0x08, 0xC0, 0xB8, 0x04, 0x76, 0xE1, 0xC5, 0xFD, 0x47, 0xD2, 0x45, 0x9C, 0x31, 0xEE,
            0x84, 0xD0, 0x37, 0x1D, 0x1C, 0x97, 0xD2, 0xBC, 0x8C, 0x54, 0x33, 0x77, 0xAE, 0xF3, 0x74, 0x32,
            0xDC, 0x79, 0x87, 0xE4, 0xFD, 0x48, 0x74, 0xA0, 0x9B, 0x1B, 0x3E, 0xEB, 0x88, 0x95, 0x92, 0x4F,
            0x3F, 0xD2, 0xD5, 0x3F, 0x0C, 0xAE, 0x35, 0xBF, 0xBC, 0x5D, 0x3D, 0xE3, 0xDE, 0xDD, 0xC0, 0xBF,
            0x52, 0xD4, 0xC6, 0xB1, 0x76, 0xCD, 0xFE, 0x6B, 0xDF, 0x71, 0x7C, 0xEA, 0x80, 0xB4, 0x83, 0x11,
            0x1C, 0xEE, 0x5D, 0xB4, 0x3D, 0x07, 0x39, 0xA5, 0xF1, 0x82, 0x40, 0x53, 0x44, 0x45, 0x1A, 0xF6,
        };

        private static Byte[] lpBox;
        private static Byte dwIndex1;
        private static Byte dwIndex2;

        private static void iInitialize(Byte[] lpKey)
        {
            lpBox = new Byte[256];

            for (Int32 i = 0; i < 256; i++)
            {
                lpBox[i] = (Byte)i;
            }

            dwIndex1 = 0;
            dwIndex2 = 0;

            Byte b = 0;

            for (Int32 i = 0; i < 256; i++)
            {
                b = (Byte)(b + lpBox[i] + lpKey[i % lpKey.Length]);
                Byte b2 = lpBox[i];
                lpBox[i] = lpBox[b];
                lpBox[b] = b2;
            }
        }

        public static Byte[] iCryptData(Byte[] lpBuffer, Int32 dwSize)
        {
            RC4.iInitialize(lpKey);

            for (Int32 i = 0; i < dwSize; i++)
            {
                dwIndex1++;
                dwIndex2 += lpBox[dwIndex1];

                Byte b = lpBox[dwIndex1];
                lpBox[dwIndex1] = lpBox[dwIndex2];
                lpBox[dwIndex2] = b;
                
                Byte b2 = (Byte)(lpBox[dwIndex1] + lpBox[dwIndex2]);
                lpBuffer[i] = (Byte)(lpBuffer[i] ^ lpBox[b2]);
            }

            return lpBuffer;
        }
    }
}
