#region

using System;
using System.Collections.Generic;
using System.Linq;
using TCompiler.Enums;
using TCompiler.Types.CheckTypes;
using TCompiler.Types.CheckTypes.Error;

#endregion

namespace TCompiler.Compiling
{
    public static class CheckForErrors
    {
        public static IEnumerable<Error> Errors(string tCode)
        {
            return BlockErrors(tCode).Select(error => (Error) error);
        }

        private static IEnumerable<BlockError> BlockErrors(string tCode)
        {
            var results = new CountResults(tCode);
            var fin = new List<BlockError>();

            if (results.CloseBlocks > results.OpenBlocks)
                fin.Add(new BlockError(CommandType.Block, "There are too many closing Blocks!", -1,
                    ErrorType.BlockBeginningMissing));
            else if (results.OpenBlocks > results.CloseBlocks)
                fin.Add(new BlockError(CommandType.Block, "There are too many opening Blocks!", -1,
                    ErrorType.BlockEndmissing));

            if (results.CloseFortilBlocks > results.OpenForTilBlocks)
                fin.Add(new BlockError(CommandType.Block, "There are too many closing ForTil Blocks!", -1,
                    ErrorType.FortilBeginningMissing));
            else if (results.OpenBlocks > results.CloseBlocks)
                fin.Add(new BlockError(CommandType.Block, "There are too many opening ForTil Blocks!", -1,
                    ErrorType.FortilEndMissing));

            if (results.CloseIfBlocks > results.OpenIfBlocks)
                fin.Add(new BlockError(CommandType.Block, "There are too many closing If Blocks!", -1,
                    ErrorType.IfBeginningMissing));
            else if (results.OpenBlocks > results.CloseBlocks)
                fin.Add(new BlockError(CommandType.Block, "There are too many opening If Blocks!", -1,
                    ErrorType.IfEndMissing));

            if (results.CloseWhileBlocks > results.OpenWhileBlocks)
                fin.Add(new BlockError(CommandType.Block, "There are too many closing While Blocks!", -1,
                    ErrorType.WhileBeginningMissing));
            else if (results.OpenWhileBlocks > results.CloseWhileBlocks)
                fin.Add(new BlockError(CommandType.Block, "There are too many opening While Blocks!", -1,
                    ErrorType.WhileEndMissing));

            if (results.CloseMethod > results.OpenMethod)
                fin.Add(new BlockError(CommandType.Block, "There are too many closing Methods!", -1,
                    ErrorType.MethodBeginningMissing));
            else if (results.OpenMethod > results.CloseMethod)
                fin.Add(new BlockError(CommandType.Block, "There are too many opening Methods!", -1,
                    ErrorType.MethodEndMissing));

            return fin;
        }

        public static int CountCommand(CommandType ct, string tCode)
            =>
            tCode.Split('\n')
                .Count(
                    s => s.Split(' ', '[').Any(s1 => s1.Equals(GetTCode(ct), StringComparison.CurrentCultureIgnoreCase)));


        private static string GetTCode(CommandType ct)
        {
            switch (ct)
            {
                case CommandType.IfBlock:
                    return "if";
                case CommandType.EndIf:
                    return "endif";
                case CommandType.WhileBlock:
                    return "while";
                case CommandType.EndWhile:
                    return "endwhile";
                case CommandType.Block:
                    return "block";
                case CommandType.EndBlock:
                    return "endblock";
                case CommandType.ForTilBlock:
                    return "fortil";
                case CommandType.EndForTil:
                    return "endfortil";
                case CommandType.Method:
                    return "method";
                case CommandType.EndMethod:
                    return "endmethod";
                default:
                    return "";
            }
        }
    }
}