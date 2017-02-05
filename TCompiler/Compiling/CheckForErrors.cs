#region

using System;
using System.Collections.Generic;
using System.Linq;
using TCompiler.Enums;
using TCompiler.Types.CheckTypes;
using TCompiler.Types.CheckTypes.Error;
using TCompiler.Types.CompilerTypes;

#endregion

namespace TCompiler.Compiling
{
    /// <summary>
    ///     Checks the provided TCode for pre-compile errors
    /// </summary>
    public static class CheckForErrors
    {
        /// <summary>
        ///     All pre-compile errors
        /// </summary>
        /// <returns>All the pre-compile errors as  list of Error</returns>
        /// <param name="tCode">The TCode that should get checked</param>
        public static IEnumerable<Error> Errors(List<List<CodeLine>> tCode)
            => BlockErrors(tCode).Select(error => (Error) error).Concat(BraceErrors(tCode));

        /// <summary>
        ///     Checks for brace errors
        /// </summary>
        /// <param name="tCode">The code to check</param>
        /// <returns>The assembler code to execute as a string</returns>
        /// <example>More open braces than closing, more closing braces than opening, wrong brace order</example>
        private static IEnumerable<BraceError> BraceErrors(List<List<CodeLine>> tCode)
        {
            var fin = new List<BraceError>();
            var openingCount = 0;
            var closingCount = 0;

            foreach (var file in tCode)
                foreach (var line in file)
                {
                    foreach (var c in line.Line)
                        switch (c)
                        {
                            case '(':
                                openingCount++;
                                break;
                            case ')':
                                closingCount++;
                                if (closingCount > openingCount)
                                    fin.Add(new BraceError(CommandType.Operation, "There is no matching opening brace!",
                                        line, ErrorType.BraceBeginningMissing));
                                break;
                        }

                    if (openingCount > closingCount)
                        fin.Add(new BraceError(CommandType.Operation, "There is no matching closing brace!",
                            line, ErrorType.BlockEndmissing));

                    openingCount = 0;
                    closingCount = 0;
                }

            return fin;
        }

        /// <summary>
        ///     Checks for block errors
        /// </summary>
        /// <remarks>e.g. too many opening blocks</remarks>
        /// <returns>The list of block errors</returns>
        /// <param name="tCode">The TCode for which the BlockErrors should get evaluated</param>
        private static IEnumerable<BlockError> BlockErrors(IEnumerable<List<CodeLine>> tCode)
        {
            var results = new CountResults(string.Join("\n", tCode.Select(file => string.Join("\n", file.Select(line => line.Line)))));
            var fin = new List<BlockError>();

            if (results.CloseBlocks > results.OpenBlocks)
                fin.Add(new BlockError(CommandType.Block, "There are too many closing Blocks!", null,
                    ErrorType.BlockBeginningMissing));
            else if (results.OpenBlocks > results.CloseBlocks)
                fin.Add(new BlockError(CommandType.Block, "There are too many opening Blocks!", null,
                    ErrorType.BlockEndmissing));

            if (results.CloseFortilBlocks > results.OpenForTilBlocks)
                fin.Add(new BlockError(CommandType.Block, "There are too many closing ForTil Blocks!", null,
                    ErrorType.FortilBeginningMissing));
            else if (results.OpenBlocks > results.CloseBlocks)
                fin.Add(new BlockError(CommandType.Block, "There are too many opening ForTil Blocks!", null,
                    ErrorType.FortilEndMissing));

            if (results.CloseIfBlocks > results.OpenIfBlocks)
                fin.Add(new BlockError(CommandType.Block, "There are too many closing If Blocks!", null,
                    ErrorType.IfBeginningMissing));
            else if (results.OpenIfBlocks > results.CloseIfBlocks)
                fin.Add(new BlockError(CommandType.Block, "There are too many opening If Blocks!", null,
                    ErrorType.IfEndMissing));

            if (results.CloseWhileBlocks > results.OpenWhileBlocks)
                fin.Add(new BlockError(CommandType.Block, "There are too many closing While Blocks!", null,
                    ErrorType.WhileBeginningMissing));
            else if (results.OpenWhileBlocks > results.CloseWhileBlocks)
                fin.Add(new BlockError(CommandType.Block, "There are too many opening While Blocks!", null,
                    ErrorType.WhileEndMissing));

            if (results.CloseMethod > results.OpenMethod)
                fin.Add(new BlockError(CommandType.Block, "There are too many closing Methods!", null,
                    ErrorType.MethodBeginningMissing));
            else if (results.OpenMethod > results.CloseMethod)
                fin.Add(new BlockError(CommandType.Block, "There are too many opening Methods!", null,
                    ErrorType.MethodEndMissing));

            return fin;
        }

        /// <summary>
        ///     Counts a provided command
        /// </summary>
        /// <returns>The amount of the command in the provided code</returns>
        /// <param name="ct">The type of the command that should be counted</param>
        /// <param name="tCode">The TCode in which the command should be counted</param>
        public static int CountCommand(CommandType ct, string tCode)
            =>
                tCode.Split('\n')
                    .Count(
                        s =>
                            s.Split(new[] { ' ', '[' }, StringSplitOptions.RemoveEmptyEntries)
                                .Any(
                                    s1 =>
                                        GetTCode(ct).Any(tc => s1.Equals(tc, StringComparison.CurrentCultureIgnoreCase))));


        /// <summary>
        ///     The possible code snippets to the given commandType
        /// </summary>
        /// <returns>The possible code snippets as a list of string</returns>
        /// <param name="ct">The type of the command for which the TCode shall get evaluated</param>
        private static IEnumerable<string> GetTCode(CommandType ct)
        {
            switch (ct)
            {
                case CommandType.IfBlock:
                    return new[] { "if" };
                case CommandType.EndIf:
                    return new[] { "endif" };
                case CommandType.WhileBlock:
                    return new[] { "while" };
                case CommandType.EndWhile:
                    return new[] { "endwhile" };
                case CommandType.Block:
                    return new[] { "block", "{" };
                case CommandType.EndBlock:
                    return new[] { "endblock", "}" };
                case CommandType.ForTilBlock:
                    return new[] { "fortil" };
                case CommandType.EndForTil:
                    return new[] { "endfortil" };
                case CommandType.Method:
                    return new[] { "method" };
                case CommandType.EndMethod:
                    return new[] { "endmethod" };
                default:
                    return new[] { "" };
            }
        }
    }
}