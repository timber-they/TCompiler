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
    ///     An assignment that evaluates toAssign or the result of the evaluation and writes the result of this into toAssign.
    ///     <br />
    ///     Syntax:<br />
    ///     toAssing |= evaluation
    /// </summary>
    public class OrAssignment : Assignment
    {
        /// <summary>
        ///     Initiates a new OrAssignment
        /// </summary>
        /// <param name="toAssign">The variable to write the result to</param>
        /// <param name="evaluation">The stuff to execute so that the result is in the accu</param>
        /// <param name="cLine">The original T code line</param>
        public OrAssignment(Variable.Variable toAssign, ReturningCommand evaluation, CodeLine cLine) : base(toAssign,
            evaluation, cLine)
        {
        }

        /// <summary>
        ///     Evaluates the stuff to execute in assembler to make an or assignment
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public override string ToString()
        {
            if (ToAssign is ByteVariable byteVariable)
                return $"{Evaluation}\norl A, {ToAssign}\n{byteVariable.MoveAccuIntoThis()}";
            if (ToAssign is BitOfVariable)
                throw new BitOfVariableException(GlobalProperties.CurrentLine);
            return $"{Evaluation}\n" +
                   $"{AssembleCodePreviews.MoveBitTo(new Bool(new Address(0x0D0, false, 7), "c", false), GlobalProperties.Label, GlobalProperties.Label, (BitVariable) ToAssign)}" +
                   $"\norl C, 0E0h.0\nmov 0E0h.0, C\n{((BitVariable) ToAssign).MoveAcc0IntoThis()}";
        }
    }
}