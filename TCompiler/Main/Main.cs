using TCompiler.Compiling;
using TCompiler.General;
using TCompiler.Settings;

namespace TCompiler.Main
{
    public class Main
    {
        public Main(string inputPath, string outputPath)
        {
            InitializeSettings(inputPath, outputPath);
        }

        public bool CompileFile()
        {
            var compiled =
                ParseToAssembler.ParseObjectsToAssembler(ParseToObjects.ParseTCodeToCommands(InputOutput.ReadInputFile()));
            return InputOutput.WriteOutputFile(compiled);
        }

        private static void InitializeSettings(string inputPath, string outputPath)
        {
            GlobalSettings.InputPath = inputPath;
            GlobalSettings.OutputPath = outputPath;
        }
    }
}