#region

using System.IO;
using TCompiler.Settings;

#endregion

namespace TCompiler.General
{
    /// <summary>
    /// Provides some stuff for IO
    /// </summary>
    public static class InputOutput
    {
        /// <summary>
        /// Reads a file
        /// </summary>
        /// <returns>The content of the file as a string</returns>
        /// <param name="path">The path from where the file shall get read</param>
        private static string ReadFile(string path)
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch (IOException)
            {
                return "";
            }
        }

        /// <summary>
        /// Reads the input file from the input file location specified in the GlobalSettings
        /// </summary>
        /// <returns>The content of the input file as a string</returns>
        public static string ReadInputFile() => ReadFile(GlobalSettings.InputPath);

        /// <summary>
        /// Writes a file to the specified path
        /// </summary>
        /// <returns>Wether this was successful</returns>
        /// <param name="path">The path to where the text shall get written</param>
        /// <param name="text">The text that shall get written</param>
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

        /// <summary>
        /// Writes the given output to the in GlobalSettings specified output path
        /// </summary>
        /// <returns>Wether this was successful</returns>
        /// <param name="text">The text that shall get written</param>
        public static bool WriteOutputFile(string text)
                    => WriteFile(GlobalSettings.OutputPath, text) && WriteFile(GlobalSettings.ErrorPath, "");

        /// <summary>
        /// Writes the given error to the in GlobalSettings defined error path
        /// </summary>
        /// <returns>Wether this was successful</returns>
        /// <param name="error">The error text that shall get written</param>
        public static bool WriteErrorFile(string error)
                    => WriteFile(GlobalSettings.OutputPath, "") && WriteFile(GlobalSettings.ErrorPath, error);
    }
}