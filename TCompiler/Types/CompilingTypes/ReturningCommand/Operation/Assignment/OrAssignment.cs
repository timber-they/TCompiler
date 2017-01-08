#region

using TCompiler.AssembleHelp;
using TCompiler.Compiling;
using TCompiler.Types.CheckTypes.TCompileException;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.Assignment
{
    public class OrAssignment : Assignment
    {
        public OrAssignment(Variable.Variable toAssign, ReturningCommand evaluation) : base(toAssign, evaluation)
        {
        }

        public override string ToString()
        {
            if (ToAssign is ByteVariable) return $"{Evaluation}\norl A, {ToAssign}\nmov {ToAssign}, A";
            if (ToAssign is BitOfVariable)
                throw new BitOfVariableException(ParseToAssembler.Line);
            return $"{Evaluation}\n" +
                   $"{AssembleCodePreviews.MoveBitTo(new Bool(false, "C", "c"), ParseToAssembler.Label, ParseToAssembler.Label, (BitVariable) ToAssign)}" +
                   $"\norl C, acc.0\nmov acc.0, C\n{((BitVariable) ToAssign).MoveAcc0IntoThis()}";
        }
    }
}