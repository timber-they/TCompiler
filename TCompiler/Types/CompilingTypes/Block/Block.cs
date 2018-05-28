#region

using System.Collections.Generic;

using TCompiler.Types.CompilerTypes;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion


namespace TCompiler.Types.CompilingTypes.Block
{
    /// <summary>
    ///     A block in which variables can exist<br />
    ///     Syntax:<br />
    ///     block
    /// </summary>
    /// <remarks>
    ///     It can start/end with {/} and with startBlock/endBlock. It's the base class for other blocks as well
    /// </remarks>
    public class Block : Command
    {
        /// <summary>
        ///     The variables that exist in this block.
        /// </summary>
        /// <remarks>Later the counter will get decreased again</remarks>
        public readonly List <Variable> Variables;

        /// <summary>
        ///     Initiates a new block
        /// </summary>
        /// <returns>Nothing</returns>
        /// <param name="endLabel">The end label of the block</param>
        /// <param name="cLine">The original T code line</param>
        public Block (Label endLabel, CodeLine cLine)
            : base (false, false, cLine)
        {
            Variables = new List <Variable> ();
            EndLabel  = endLabel;
        }

        /// <summary>
        ///     The end label so u can always end the current block with a break
        /// </summary>
        /// <value>The Label as a label. To call it jmp label.ToString() is used</value>
        public Label EndLabel { get; set; }
    }
}