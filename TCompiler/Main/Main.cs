using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using TCompiler.Compiling;
using TCompiler.General;
using TCompiler.Settings;
using TCompiler.Types.CheckTypes.Error;
using TCompiler.Types.CheckTypes.TCompileException;

namespace TCompiler.Main
{
    public class Main
    {
        public Main(string inputPath, string outputPath)
        {
            InitializeSettings(inputPath, outputPath);
        }

        public static bool CompileFile()
        {
            var errors = new List<Error>();
            try
            {
                var code = InputOutput.ReadInputFile();
                errors = CheckForErrors.Errors(code).ToList();
                if(errors.Any())
                    throw new NormalErrorException(errors.FirstOrDefault());
                var compiled =
                    ParseToAssembler.ParseObjectsToAssembler(ParseToObjects.ParseTCodeToCommands(code));
                return InputOutput.WriteOutputFile(compiled);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Some errors occurred:\n{e}");
                for (var i = 1; i < errors.Count; i++)
                    Console.WriteLine(errors[i].Message);
                return false;
            }
        }

        private static void InitializeSettings(string inputPath, string outputPath)
        {
            GlobalSettings.InputPath = inputPath;
            GlobalSettings.OutputPath = outputPath;
        }
    }
}