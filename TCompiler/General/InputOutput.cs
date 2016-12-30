using System.IO;
using TCompiler.Settings;

namespace TCompiler.General
{
    public static class InputOutput
    {
        private static string ReadFile(string path)
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch (IOException e)
            {
                return "";
            }
        }

        public static string ReadInputFile() => ReadFile(GlobalSettings.InputPath);

        private static bool WriteFile(string path, string text)
        {
            try
            {
                File.WriteAllText(path, text);
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }

        public static bool WriteOutputFile(string text)
            => WriteFile(GlobalSettings.OutputPath, text) && WriteFile(GlobalSettings.ErrorPath, "");

        public static bool WriteErrorFile(string error)
            => WriteFile(GlobalSettings.OutputPath, "") && WriteFile(GlobalSettings.ErrorPath, error);
    }
}