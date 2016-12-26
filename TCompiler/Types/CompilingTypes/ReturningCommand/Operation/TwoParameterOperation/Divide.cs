using System;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation
{
    public class Divide : TwoParameterOperation
    {
        public Divide(ByteVariableCall paramA, ByteVariableCall paramB) : base(paramA, paramB)
        {
        }

        public Divide(Tuple<ByteVariableCall, ByteVariableCall> pars) : base(pars.Item1, pars.Item2) { }
        public override string ToString() => $"{_paramA}\nmov B, {((ByteVariableCall)_paramB).Variable}\ndiv AB";
    }
}