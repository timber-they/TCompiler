#region

using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.Assignment
{
    public class OrAssignment : Assignment
    {
        public OrAssignment(Variable.Variable toAssign, ReturningCommand evaluation) : base(toAssign, evaluation)
        {
        }

        public override string ToString() => ToAssign is ByteVariable
            ? $"{Evaluation}\norl A, {ToAssign}\nmov {ToAssign}, A"
            : $"{Evaluation}\nmov C, {ToAssign}\norl C, acc.0\nmov C, acc.0\nmov {ToAssign}, C";
    }
}