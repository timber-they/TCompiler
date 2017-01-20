namespace TestApplication
{
    internal static class Program
    {
        private static void Main()
        {
            //This is just a test application - I commented it for the code metrics result.
            //a:
            //var writing = true;
            //var sb = new StringBuilder();
            //c:

            //Console.CancelKeyPress += (sender, eventArgs) =>
            //{
            //    eventArgs.Cancel = true;
            //    writing = false;
            //};
            //if (writing)
            //{
            //    var s = Console.ReadLine();
            //    if (s == "compile")
            //    {
            //        writing = false;
            //        goto b;
            //    }
            //    sb.AppendLine(s);

            //    goto c;
            //}
            //b:

            //File.WriteAllText("in.tc", sb.ToString());

            //Console.Clear();
            //TCompiler.Main.Main.Initialize("in.tc", "out.asm", "error.txt");
            //if (TCompiler.Main.Main.CompileFile() != null)
            //{
            //    Console.WriteLine("ERROR!");
            //    goto a;
            //}
            //Console.WriteLine(File.ReadAllText(GlobalProperties.ErrorPath));
            //Console.WriteLine(File.ReadAllText(GlobalProperties.OutputPath));
            //goto a;
        }
    }
}