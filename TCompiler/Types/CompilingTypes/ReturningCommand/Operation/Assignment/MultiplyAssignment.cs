namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.Assignment
{
    public class MultiplyAssignment : Assignment
    {
        public MultiplyAssignment(Variable.Variable toAssign, ReturningCommand evaluation) : base(toAssign, evaluation)
        {
        }

        public override string ToString() => $"{Evaluation}\nmov B, {ToAssign}\nmul AB\nmov {ToAssign}, A";
    }
}