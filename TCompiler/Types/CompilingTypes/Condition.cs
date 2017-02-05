using TCompiler.Types.CompilerTypes;

namespace TCompiler.Types.CompilingTypes
{
    /// <summary>
    ///     A condition, used for while and if blocks
    /// </summary>
    public class Condition : Command
    {
        /// <summary>
        ///     Initiates a new condition
        /// </summary>
        /// <returns>Nothing</returns>
        public Condition(ReturningCommand.ReturningCommand evaluation, CodeLine tCode)
            : base(true, true, tCode)
        {
            Evaluation = evaluation;
        }

        /// <summary>
        ///     The stuff that has to get executed before the result is in 0E0h.0
        /// </summary>
        /// <value>A returning command (should be a boolean value)</value>
        public ReturningCommand.ReturningCommand Evaluation { get; }

        /// <summary>
        ///     The condition as assembler code
        /// </summary>
        /// <returns>Returns the assembler code that must get executed so that the result of the Evaluation is in 0E0h.0</returns>
        public override string ToString() => Evaluation.ToString();
    }
}