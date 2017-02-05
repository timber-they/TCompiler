#region

using TCompiler.AssembleHelp;
using TCompiler.Settings;
using TCompiler.Types.CheckTypes.TCompileException;
using TCompiler.Types.CompilerTypes;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.Assignment
{
    /// <summary>
    ///     An assignment that writes the result of toAssign & evaluation into toAssign<br />
    ///     Syntax:<br />
    ///     toAssing &= evaluation
    /// </summary>
    public class AndAssignment : Assignment
    {
        /// <summary>
        ///     Initiates a new AndAssignment
        /// </summary>
        /// <param name="toAssign">The variable to assign the result</param>
        /// <param name="evaluation">The stuff to execute before the andAssignment</param>
        /// <param name="cLine">The original T code line</param>
        public AndAssignment(Variable.Variable toAssign, ReturningCommand evaluation, CodeLine cLine) : base(toAssign, evaluation, cLine)
        {
        }

        /// <summary>
        ///     Evaluates the code to execute in assembler
        /// </summary>
        /// <returns>The code as a string</returns>
        public override string ToString()
        {
            if (ToAssign is ByteVariable)
                return $"{Evaluation}\norl A, {ToAssign}\n{((ByteVariable) ToAssign).MoveAccuIntoThis()}";
            if (ToAssign is BitOfVariable)
                throw new BitOfVariableException(GlobalProperties.CurrentLine);
            return $"{Evaluation}\n" +
                   $"{AssembleCodePreviews.MoveBitTo(new Bool(new Address(0x0D0, false, 7), "c", false), GlobalProperties.Label, GlobalProperties.Label, (BitVariable) ToAssign)}\n" +
                   "anl C, 0E0h.0\n" +
                   "mov 0E0h.0, C\n" +
                   $"{((BitVariable) ToAssign).MoveAcc0IntoThis()}";
        }
    }
}