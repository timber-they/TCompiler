using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCompiler.Main;

namespace TestApplication
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var writing = true;
            var sb = new StringBuilder();

            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                eventArgs.Cancel = true;
                writing = false;
            };

            a:
            if (writing)
            {
                sb.AppendLine(Console.ReadLine());
                goto a;
            }

            File.WriteAllText("in.tc", sb.ToString());

            Console.Clear();
            var m = new Main("in.tc", "out.asm");
            if (!m.CompileFile())
            {
                Console.WriteLine("ERROR!");
                goto a;
            }
            Console.WriteLine(File.ReadAllText("out.asm"));
            Console.ReadLine();
            goto a;
        }
    }
}
