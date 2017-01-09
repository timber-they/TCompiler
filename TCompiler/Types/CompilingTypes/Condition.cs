namespace TCompiler.Types.CompilingTypes
{
    /// <summary>
    /// A condition, used for while and if blocks
    /// </summary>
    public class Condition : Command
    {
        /// <summary>
        /// Initiates a new condition
        /// </summary>
        /// <returns>Nothing</returns>
        public Condition(ReturningCommand.ReturningCommand evaluation) : base (false)
        {
            Evaluation = evaluation;
        }

        /// <summary>
        /// The stuff that has to get executed before the result is in acc.0
        /// </summary>
        /// <value>A returning command (should be a boolean value)</value>
        private ReturningCommand.ReturningCommand Evaluation { get; }

        /// <summary>
        /// The condition as assembler code
        /// </summary>
        /// <returns>Returns the assembler code that must get executed so that the result of the Evaluation is in acc.0</returns>
        public override string ToString() => Evaluation.ToString();
    }
}