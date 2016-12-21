namespace TCompiler.Types.CompilingTypes.Operation
{
    public class TwoParameterOperation : Operation
    {
        private Variable.Variable _paramA;
        private Variable.Variable _paramB;

        public TwoParameterOperation(Variable.Variable paramA, Variable.Variable paramB)
        {
            _paramA = paramA;
            _paramB = paramB;
        }
    }
}