using System;
using System.Text;
using TCompiler.Compiling;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.Compare
{
    public abstract class Compare : TwoParameterOperation
    {
        protected Compare(Variable.Variable paramA, Variable.Variable paramB) : base(paramA, paramB)
        {
        }

        protected Compare(Tuple<Variable.Variable, Variable.Variable> pars) : base(pars)
        {
        }

        public abstract override string ToString();
    }
}