namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation
{
    public abstract class OneParameterOperation : Operation
    {
        private Variable.Variable _paramA;

        protected OneParameterOperation(Variable.Variable paramA)
        {
            _paramA = paramA;
        }
    }
}