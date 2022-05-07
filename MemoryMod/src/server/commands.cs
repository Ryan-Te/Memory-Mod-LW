using LICC;
using System;
using System.IO;

namespace Ryan.Memory
{
    public class commands
    {
        [Command("TestEcho")]
        static void TestEcho(string s)
        {
            LConsole.WriteLine(Directory.GetCurrentDirectory().Substring(0, Directory.GetCurrentDirectory().Length - 7));
        }

        [Command("setRomLoc")]
        static void setRomLoc(string s)
        {
            Memory_Mod.FlashLocationGlobal = "/" + s;
        }
    }
}