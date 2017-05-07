#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        /// <param name="inputPath">The main input path</param>
        /// <param name="outputPath">The path for the output file</param>
        /// <param name="errorPath">The path for the error file</param>
        private static void Initialize(string inputPath, string outputPath, string errorPath) => InitializeSettings(
            inputPath, outputPath, errorPath);

        private static List<string> GetInputPaths(string inputPath)
        {
            if (!File.Exists(inputPath))
                throw new FileDoesntExistException(null, inputPath);
            var fin = new List<string> { inputPath };
            foreach (var line in File.ReadAllLines(inputPath).Select(s => s.Trim()))
                if (line.StartsWith("include ", StringComparison.CurrentCultureIgnoreCase))
                    fin.AddRange(GetInputPaths(line.Substring(line.Split(' ').First().Length + 1)));
            return fin;
        }

        /// <summary>
        ///     Compiles the file to assembler
        /// </summary>
        /// <returns>The first compile exception that was thrown</returns>
        public static CompileException CompileFile(string inputPath, string outputPath, string errorPath, bool optimize = false)
        {
            var errors = new List<Error>();
            try
            {
                Initialize(inputPath, outputPath, errorPath);
                var tCode = InputOutput.ReadInputFiles();
                var modified = Modifying.GetModifiedTCode(tCode);
                errors = CheckForErrors.Errors(modified).ToList();
                if (errors.Any())
                    throw new PreCompileErrorException(errors.FirstOrDefault());
                var compiled =
                    ParseToAssembler.ParseObjectsToAssembler(ParseToObjects.ParseTCodeToCommands(modified));
                InputOutput.WriteOutputFile(optimize ? Optimizing.GetOptimizedAssemblerCode(compiled) : compiled);
                return null;
            }
            catch (Exception e)
            {
                var frame = new StackTrace(e, true).GetFrames()?.FirstOrDefault();
                var compileException = e as CompileException ??
                                       new InternalException(e.Message, frame?.GetFileLineNumber(), frame?.GetFileName());
                var sb = new StringBuilder();
                sb.AppendLine($"An error occurred in {compileException.CodeLine?.FileName ?? "your project"}\nAt line {compileException.CodeLine?.LineIndex.ToString() ?? "??"}:\n{compileException.Message}");
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
            GlobalProperties.OutputPath = outputPath;
            GlobalProperties.ErrorPath = errorPath;
            GlobalProperties.InputPaths = GetInputPaths(inputPath);
        }
    }
}