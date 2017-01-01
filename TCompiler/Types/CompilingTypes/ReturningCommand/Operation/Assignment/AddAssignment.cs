namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.Assignment
{
    public class AddAssignment : Assignment
    {
        public AddAssignment(Variable.Variable toAssign, ReturningCommand evaluation) : base(toAssign, evaluation)
        {
        }

        public override string ToString() => $"{Evaluation}\nadd A, {ToAssign}\nmov {ToAssign}, A";
    }
}