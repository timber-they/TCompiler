namespace TCompiler.Types.CompilingTypes.Operation
{
    public class OneParameterOperation : Operation
    {
        private Variable.Variable _paramA;

        public OneParameterOperation(Variable.Variable paramA)
        {
            _paramA = paramA;
        }
    }
}