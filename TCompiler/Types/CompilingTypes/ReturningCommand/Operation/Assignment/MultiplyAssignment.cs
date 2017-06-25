#region

using TCompiler.Types.CompilerTypes;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion


namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.Assignment
{
    /// <summary>
    ///     An assignment that multiplies the result of the evaluation and toAssign and writes the result of this into toAssing
    ///     <br />
    ///     Syntax:<br />
    ///     toAssign *= evaluation
    /// </summary>
    public class MultiplyAssignment : Assignment
    {
        /// <summary>
        ///     Initiates a new MultiplyAssignment
        /// </summary>
        /// <param name="toAssign">The variable to assign the result</param>
        /// <param name="evaluation">The stuff to execute so that the result is in the Accu</param>
        /// <param name="cLine">The original T code line</param>
        public MultiplyAssignment (Variable.Variable toAssign, ReturningCommand evaluation, CodeLine cLine) : base (
            toAssign, evaluation, cLine) {}

        /// <summary>
        ///     Evaluates the stuff to execute in assembler to make a multiply assignment
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public override string ToString ()
            =>
                $"{Evaluation}\n{((ByteVariable) ToAssign).MoveThisIntoB ()}\nmul AB\n{((ByteVariable) ToAssign).MoveAccuIntoThis ()}";
    }
}