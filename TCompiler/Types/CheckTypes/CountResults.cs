#region

using TCompiler.Compiling;
using TCompiler.Enums;

#endregion

namespace TCompiler.Types.CheckTypes
{
    public class CountResults
    {
        public CountResults(string tCode)
        {
            OpenBlocks = CheckForErrors.CountCommand(CommandType.Block, tCode);
            CloseBlocks = CheckForErrors.CountCommand(CommandType.EndBlock, tCode);
            OpenIfBlocks = CheckForErrors.CountCommand(CommandType.IfBlock, tCode);
            CloseIfBlocks = CheckForErrors.CountCommand(CommandType.EndIf, tCode);
            OpenWhileBlocks = CheckForErrors.CountCommand(CommandType.WhileBlock, tCode);
            CloseWhileBlocks = CheckForErrors.CountCommand(CommandType.EndWhile, tCode);
            OpenForTilBlocks = CheckForErrors.CountCommand(CommandType.ForTilBlock, tCode);
            CloseFortilBlocks = CheckForErrors.CountCommand(CommandType.EndForTil, tCode);
            OpenMethod = CheckForErrors.CountCommand(CommandType.Method, tCode);
            CloseMethod = CheckForErrors.CountCommand(CommandType.EndMethod, tCode);
        }

        public int OpenBlocks { get; }
        public int CloseBlocks { get; }

        public int OpenIfBlocks { get; }
        public int CloseIfBlocks { get; }

        public int OpenWhileBlocks { get; }
        public int CloseWhileBlocks { get; }

        public int OpenForTilBlocks { get; }
        public int CloseFortilBlocks { get; }

        public int OpenMethod { get; }
        public int CloseMethod { get; }
    }
}