namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.Assignment
{
    public class ModuloAssignment : Assignment
    {
        public ModuloAssignment(Variable.Variable toAssign, ReturningCommand evaluation) : base(toAssign, evaluation)
        {
        }

        public override string ToString() => $"{Evaluation}\nmov B, {ToAssign}\nxch A, B\ndiv AB\nmov {ToAssign}, B";
    }
}