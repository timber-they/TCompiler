#region

using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.Assignment
{
    public class AndAssignment : Assignment
    {
        public AndAssignment(Variable.Variable toAssign, ReturningCommand evaluation) : base(toAssign, evaluation)
        {
        }

        public override string ToString() => ToAssign is ByteVariable
            ? $"{Evaluation}\nanl A, {ToAssign}\nmov {ToAssign}, A"
            : $"{Evaluation}\nmov C, {ToAssign}\nanl C, acc.0\nmov C, acc.0\nmov {ToAssign}, C";
    }
}