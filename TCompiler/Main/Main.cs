using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCompiler.Compiling;
using TCompiler.General;
using TCompiler.Settings;
using TCompiler.Types.CheckTypes.Error;
using TCompiler.Types.CheckTypes.TCompileException;

namespace TCompiler.Main
{
    public static class Main
    {
        public static void Initialize(string inputPath, string outputPath, string errorPath)
        {
            InitializeSettings(inputPath, outputPath, errorPath);
        }

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
                    ParseToAssembler.ParseObjectsToAssembler(ParseToObjects.ParseTCodeToCommands(code));
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

        private static void InitializeSettings(string inputPath, string outputPath, string errorPath)
        {
            GlobalSettings.InputPath = inputPath;
            GlobalSettings.OutputPath = outputPath;
            GlobalSettings.ErrorPath = errorPath;
        }
    }
}