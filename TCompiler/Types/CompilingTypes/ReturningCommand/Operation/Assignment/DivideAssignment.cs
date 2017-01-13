namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.Assignment
{
    /// <summary>
    /// An assignment that divides toAssign with the result from the evaluation and writes the result of this in toAssign<br/>
    /// Syntax:<br/>
    /// toAssign /= evaluation
    /// </summary>
    public class DivideAssignment : Assignment
    {
        /// <summary>
        /// Initiates a new DivideAssignment
        /// </summary>
        /// <param name="toAssign">The variable to assign the result to</param>
        /// <param name="evaluation">The stuff to execute so the result is in the Accu</param>
        public DivideAssignment(Variable.Variable toAssign, ReturningCommand evaluation) : base(toAssign, evaluation)
        {
        }

        /// <summary>
        /// Evaluates the stuff to execute in assembler to make a divide assignment
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{Evaluation}\nmov B, {ToAssign}\nxch A, B\ndiv AB\nmov {ToAssign}, A";
    }
}