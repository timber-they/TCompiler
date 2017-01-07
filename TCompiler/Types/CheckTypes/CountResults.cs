#region

using TCompiler.Compiling;
using TCompiler.Enums;

#endregion

namespace TCompiler.Types.CheckTypes
{
    /// <summary>
    /// The results of the countChecking
    /// </summary>
    public class CountResults
    {
        /// <summary>
        /// Initiates a new countResults object and counts the commands
        /// </summary>
        /// <returns>Nothing</returns>
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

        /// <summary>
        /// The amount of normal opening blocks
        /// </summary>
        /// <value>The amount as an integer</value>
        public int OpenBlocks { get; }
        /// <summary>
        /// The amount of normal closing blocks
        /// </summary>
        /// <value>The amount as an integer</value>
        public int CloseBlocks { get; }

        /// <summary>
        /// The amount of opening if blocks
        /// </summary>
        /// <value>The amount as an integer</value>
        public int OpenIfBlocks { get; }
        /// <summary>
        /// The amount of closing if blocks
        /// </summary>
        /// <value>The amount as an integer</value>
        public int CloseIfBlocks { get; }

        /// <summary>
        /// The amount of opening while blocks
        /// </summary>
        /// <value>The amount as an integer</value>
        public int OpenWhileBlocks { get; }
        /// <summary>
        /// The amount of closing while blocks
        /// </summary>
        /// <value>The amount as an integer</value>
        public int CloseWhileBlocks { get; }

        /// <summary>
        /// The amount of opening forTil blocks
        /// </summary>
        /// <value>The amount as an integer</value>
        public int OpenForTilBlocks { get; }
        /// <summary>
        /// The amount of closing forTil blocks
        /// </summary>
        /// <value>The amount as an integer</value>
        public int CloseFortilBlocks { get; }

        /// <summary>
        /// The amount of opening methods
        /// </summary>
        /// <value>The amount as an integer</value>
        public int OpenMethod { get; }
        /// <summary>
        /// The amount of closing methods
        /// </summary>
        /// <value>The amount as an integer</value>
        public int CloseMethod { get; }
    }
}