#region

using TCompiler.AssembleHelp;
using TCompiler.Compiling;
using TCompiler.Types.CheckTypes.TCompileException;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.Assignment
{
    /// <summary>
    /// An assignment that writes the result of toAssign & evaluation into toAssign<br/>
    /// Syntax:<br/>
    /// toAssing &= evaluation
    /// </summary>
    public class AndAssignment : Assignment
    {
        /// <summary>
        /// Initiates a new AndAssignment
        /// </summary>
        /// <param name="toAssign">The variable to assign the result</param>
        /// <param name="evaluation">The stuff to execute before the andAssignment</param>
        public AndAssignment(Variable.Variable toAssign, ReturningCommand evaluation) : base(toAssign, evaluation)
        {
        }

        /// <summary>
        /// Evaluates the code to execute in assembler
        /// </summary>
        /// <returns>The code as a string</returns>
        public override string ToString()
        {
            if (ToAssign is ByteVariable)
                return $"{Evaluation}\norl A, {ToAssign}\nmov {ToAssign}, A";
            if (ToAssign is BitOfVariable)
                throw new BitOfVariableException(ParseToAssembler.Line);
            return $"{Evaluation}\n" +
                   $"{AssembleCodePreviews.MoveBitTo(new Bool("C", "c", false), ParseToAssembler.Label, ParseToAssembler.Label, (BitVariable) ToAssign)}" +
                   $"\nanl C, 224.0\nmov 224.0, C\n{((BitVariable) ToAssign).MoveAcc0IntoThis()}";
        }
    }
}