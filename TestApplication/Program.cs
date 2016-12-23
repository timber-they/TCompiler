using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCompiler.Main;

namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            a:
            var m = new Main("in.tc", "out.asm");
            m.CompileFile();
            Console.WriteLine(File.ReadAllText("out.asm"));
            Console.ReadLine();
            goto a;
        }
    }
}
