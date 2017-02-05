#region

using TCompiler.Types.CompilerTypes;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.Assignment
{
    /// <summary>
    ///     An assignment that subtracts the result of the evaluation from toAssign and writes the result of this into toAssign
    /// </summary>
    public class SubtractAssignment : Assignment
    {
        /// <summary>
        ///     Initiates a new SubtractAssignment
        /// </summary>
        /// <param name="toAssign">The variable where the result is written into</param>
        /// <param name="evaluation">The stuff to execute so that the result is in the accu</param>
        /// <param name="cLine">The original T code line</param>
        public SubtractAssignment(Variable.Variable toAssign, ReturningCommand evaluation, CodeLine cLine) : base(toAssign, evaluation, cLine)
        {
        }

        /// <summary>
        ///     Evaluates the stuff to execute in assembler to make a subtract assignment
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public override string ToString()
            =>
                $"{Evaluation}\nxch A, {ToAssign}\nclr C\nsubb A, {ToAssign}\n{((ByteVariable) ToAssign).MoveAccuIntoThis()}";
    }
}