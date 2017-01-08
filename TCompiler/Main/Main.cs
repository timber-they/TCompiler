#region

using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCompiler.Compiling;
using TCompiler.General;
using TCompiler.Settings;
using TCompiler.Types.CheckTypes.Error;
using TCompiler.Types.CheckTypes.TCompileException;

#endregion

namespace TCompiler.Main
{
    /// <summary>
    /// The main compile class. Provides all the compile stuff like checking, compiling to objects and compiling to assembler
    /// </summary>
    public static class Main
    {
        /// <summary>
        /// Initializes the compiling
        /// </summary>
        /// <param name="inputPath">The path for the input file</param>
        /// <param name="outputPath">The path for the output file</param>
        /// <param name="errorPath">The path for the error file</param>
        public static void Initialize(string inputPath, string outputPath, string errorPath)
        {
            InitializeSettings(inputPath, outputPath, errorPath);
        }

        /// <summary>
        /// Compiles the file to assembler
        /// </summary>
        /// <returns>The first compile exception that was thrown</returns>
        public static CompileException CompileFile()
        {
            var errors = new List<Error>();
            try
            {
                var code = InputOutput.ReadInputFile();
                errors = CheckForErrors.Errors(code).ToList();
                if (errors.Any())
                    throw new NormalErrorException(errors.FirstOrDefault());
                var compiled =
                    ParseToAssembler.ParseObjectsToAssembler(ParseToObjects.ParseTCodeToCommands(code), code.Split('\n').Select(s => s.Trim(' ', '\r')).ToArray());
                InputOutput.WriteOutputFile(compiled);
                return null;
            }
            catch (CompileException e)
            {
                var sb = new StringBuilder();
                sb.AppendLine($"An error occurred:\n{e.Message}");
                for (var i = 1; i < errors.Count; i++)
                    sb.AppendLine(errors[i].Message);
                InputOutput.WriteErrorFile(sb.ToString());
                return e;
            }
        }

        /// <summary>
        /// Initializes the GlobalSettings
        /// </summary>
        /// <param name="inputPath">The path for the input file</param>
        /// <param name="outputPath">The path for the output file</param>
        /// <param name="errorPath">The path for the error file</param>
        private static void InitializeSettings(string inputPath, string outputPath, string errorPath)
        {
            GlobalSettings.InputPath = inputPath;
            GlobalSettings.OutputPath = outputPath;
            GlobalSettings.ErrorPath = errorPath;
        }
    }
}