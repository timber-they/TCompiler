namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.Assignment
{
    /// <summary>
    ///     An assignment that makes toAssign modulo the result of the evaluation and writes the result of this into toAssign
    ///     <br />
    ///     Syntax:<br />
    ///     toAssign %= evaluation
    /// </summary>
    public class ModuloAssignment : Assignment
    {
        /// <summary>
        ///     Initiates a new ModuloAssignment
        /// </summary>
        /// <param name="toAssign">The variable to assign the result to</param>
        /// <param name="evaluation">The stuff to execute so the result is in the Accu</param>
        public ModuloAssignment(Variable.Variable toAssign, ReturningCommand evaluation) : base(toAssign, evaluation)
        {
        }

        /// <summary>
        ///     Evaluates the stuff to execute in assembler to make a modulo assignment
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public override string ToString() => $"{Evaluation}\nmov B, {ToAssign}\nxch A, B\ndiv AB\nmov {ToAssign}, B";
    }
}