using System;

namespace DSR.Unpacker
{
    class Program
    {
        private static String m_Title = "Devil Slayer: Raksasi APL Unpacker";

        static void Main(String[] args)
        {
            Console.Title = m_Title;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(m_Title);
            Console.WriteLine("(c) 2024 Ekey (h4x0r) / v{0}\n", Utils.iGetApplicationVersion());
            Console.ResetColor();

            if (args.Length != 2)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[Usage]");
                Console.WriteLine("    DSR.Unpacker <m_Alp_File> <m_OutDirectory>");
                Console.WriteLine("    m_Alp_File - Source of alp file");
                Console.WriteLine("    m_OutDirectory - Destination directory\n");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Examples]");
                Console.WriteLine("    DSR.Unpacker E:\\Games\\Raksasi\\raksasi_Data\\StreamingAssets\\Lua.alp D:\\Unpacked");
                Console.ResetColor();
                return;
            }

            String m_Input = args[0];
            String m_Output = Utils.iCheckArgumentsPath(args[1]);

            AlpUnpack.iDoIt(m_Input, m_Output);
        }
    }
}
