using TCompiler.Settings;

namespace TCompiler.Main
{
    public class Main
    {
        public Main(string inputPath, string outputPath)
        {
            InitializeSettings(inputPath, outputPath);
        }

        private void InitializeSettings(string inputPath, string outputPath)
        {
            GlobalSettings.InputPath = inputPath;
            GlobalSettings.OutputPath = outputPath;
        }
    }
}