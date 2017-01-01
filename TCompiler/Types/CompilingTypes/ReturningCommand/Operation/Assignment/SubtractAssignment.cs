namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.Assignment
{
    public class SubtractAssignment : Assignment
    {
        public SubtractAssignment(Variable.Variable toAssign, ReturningCommand evaluation) : base(toAssign, evaluation)
        {
        }

        public override string ToString()
            => $"{Evaluation}\nxch A, {ToAssign}\nclr C\nsubb A, {ToAssign}\nmov {ToAssign}, A";
    }
}