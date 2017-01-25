#region

using System;
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
    ///     The main compile class. Provides all the compile stuff like checking, compiling to objects and compiling to
    ///     assembler
    /// </summary>
    public static class Main
    {
        /// <summary>
        ///     Initializes the compiling
        /// </summary>
        /// <param name="inputPath">The path for the input file</param>
        /// <param name="outputPath">The path for the output file</param>
        /// <param name="errorPath">The path for the error file</param>
        public static void Initialize(string inputPath, string outputPath, string errorPath)
        {
            InitializeSettings(inputPath, outputPath, errorPath);
        }

        /// <summary>
        ///     Compiles the file to assembler
        /// </summary>
        /// <returns>The first compile exception that was thrown</returns>
        public static CompileException CompileFile(bool optimize = false)
        {
            var errors = new List<Error>();
            try
            {
                var code = InputOutput.ReadInputFile();
                errors = CheckForErrors.Errors(code).ToList();
                if (errors.Any())
                    throw new PreCompileErrorException(errors.FirstOrDefault());
                var compiled =
                    ParseToAssembler.ParseObjectsToAssembler(ParseToObjects.ParseTCodeToCommands(code),
                        code.Split('\n').Select(s => s.Trim(' ', '\r')).ToArray());
                InputOutput.WriteOutputFile(optimize ? Optimizing.GetOptimizedAssemblerCode(compiled) : compiled);
                return null;
            }
            catch (CompileException e)
            {
                var compileException = e as CompileException ?? new InternalException(e.Message);
                var sb = new StringBuilder();
                sb.AppendLine($"An error occurred:\n{compileException.Message}");
                for (var i = 1; i < errors.Count; i++)
                    sb.AppendLine(errors[i].Message);
                InputOutput.WriteErrorFile(sb.ToString());
                return compileException;
            }
        }

        /// <summary>
        ///     Initializes the GlobalProperties
        /// </summary>
        /// <param name="inputPath">The path for the input file</param>
        /// <param name="outputPath">The path for the output file</param>
        /// <param name="errorPath">The path for the error file</param>
        private static void InitializeSettings(string inputPath, string outputPath, string errorPath)
        {
            GlobalProperties.InputPath = inputPath;
            GlobalProperties.OutputPath = outputPath;
            GlobalProperties.ErrorPath = errorPath;
        }
    }
}