#region

using TCompiler.Types.CompilerTypes;
using TCompiler.Types.CompilingTypes.ReturningCommand.Operation.Assignment;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    /// <summary>
    ///     A normal variable declaration
    /// </summary>
    public class Declaration : Command
    {
        /// <summary>
        ///     Initiates a new declaration command
        /// </summary>
        /// <returns>Nothing</returns>
        /// <param name="tCode">The original T code line</param>
        /// <param name="assignment">The assignment that may be included in the declaration</param>
        public Declaration(CodeLine tCode, Assignment assignment = null)
            : base(
                true, true, tCode)
        {
            Assignment = assignment;
        }

        /// <summary>
        ///     Doesn't have to be here, but the variable can be assigned directly
        /// </summary>
        /// <value>The assignment as a returning command</value>
        private Assignment Assignment { get; }

        /// <summary>
        ///     Makes the result of the assignment stand in the Accu
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public override string ToString() => Assignment?.ToString() ?? "";
    }
}