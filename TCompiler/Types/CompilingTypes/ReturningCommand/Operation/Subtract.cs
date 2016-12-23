using System;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation
{
    public class Subtract : TwoParameterOperation
    {
        public Subtract(Variable.Variable paramA, Variable.Variable paramB) : base(paramA, paramB)
        {
        }

        public Subtract(Tuple<Variable.Variable, Variable.Variable> pars) : base(pars) { }
        public override string ToString() => $"clr C\nmov A, {_paramA}\nsubb A, {_paramB}";
    }
}