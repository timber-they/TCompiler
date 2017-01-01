namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.Assignment
{
    public class DivideAssignment : Assignment
    {
        public DivideAssignment(Variable.Variable toAssign, ReturningCommand evaluation) : base(toAssign, evaluation)
        {
        }

        public override string ToString() => $"{Evaluation}\nmov B, {ToAssign}\nxch A, B\ndiv AB\nmov {ToAssign}, A";
    }
}