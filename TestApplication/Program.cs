using System;
using System.IO;
using System.Text;
using TCompiler.Settings;

namespace TestApplication
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            a:
            var writing = true;
            var sb = new StringBuilder();
            c:

            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                eventArgs.Cancel = true;
                writing = false;
            };
            if (writing)
            {
                var s = Console.ReadLine();
                if (s == "compile")
                {
                    writing = false;
                    goto b;
                }
                sb.AppendLine(s);

                goto c;
            }
            b:

            File.WriteAllText("in.tc", sb.ToString());

            Console.Clear();
            TCompiler.Main.Main.Initialize("in.tc", "out.asm", "error.txt");
            if (TCompiler.Main.Main.CompileFile() != null)
            {
                Console.WriteLine("ERROR!");
                goto a;
            }
            Console.WriteLine(File.ReadAllText(GlobalSettings.ErrorPath));
            Console.WriteLine(File.ReadAllText(GlobalSettings.OutputPath));
            goto a;
        }
    }
}