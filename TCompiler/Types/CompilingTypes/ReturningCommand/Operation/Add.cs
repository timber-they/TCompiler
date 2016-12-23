using System;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation
{
    public class Add : TwoParameterOperation
    {
        public Add(Variable.Variable paramA, Variable.Variable paramB) : base(paramA, paramB)
        {
        }

        public Add(Tuple<Variable.Variable, Variable.Variable> pars) : base(pars) { }
        public override string ToString() => $"mov A, {_paramA}\nadd A, {_paramB}";
    }
}