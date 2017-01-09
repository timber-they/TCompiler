namespace TCompiler.Types.CompilingTypes.ReturningCommand
{
    /// <summary>
    /// The base class for every command that returns something.
    /// </summary>
    /// <remarks>In the end the result will be saved in the Accu or in the first bit (acc.0) of the Accu</remarks>
    public abstract class ReturningCommand : Command
    {
        /// <summary>
        /// Every returning command can get converted to assembler directly with the toString method
        /// </summary>
        /// <remarks>Every inheriting class has to implement this method</remarks>
        /// <returns>The assembler code for the returning command</returns>
        public abstract override string ToString();

        protected ReturningCommand(bool isSingleLine) : base(isSingleLine)
        {
        }
    }
}