namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation
{
    public class Assignment : Operation
    {
        private Variable.Variable _toAssign;
        private ReturningCommand _evalutation;

        public Assignment(Variable.Variable toAssign, ReturningCommand evalutation)
        {
            _toAssign = toAssign;
            _evalutation = evalutation;
        }
    }
}