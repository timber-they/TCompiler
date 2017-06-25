#region

using TCompiler.Types.CompilerTypes;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion


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
        /// <param name="cLine">The original T code line</param>
        public ModuloAssignment (Variable.Variable toAssign, ReturningCommand evaluation, CodeLine cLine) : base (
            toAssign, evaluation, cLine) {}

        /// <summary>
        ///     Evaluates the stuff to execute in assembler to make a modulo assignment
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public override string ToString ()
            =>
                $"{Evaluation}\n{((ByteVariable) ToAssign).MoveThisIntoB ()}\nxch A, B\ndiv AB\n{((ByteVariable) ToAssign).MoveBIntoThis ()}";
    }
}