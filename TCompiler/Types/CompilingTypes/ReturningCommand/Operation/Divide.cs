using System;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation
{
    public class Divide : TwoParameterOperation
    {
        public Divide(ByteVariableCall paramA, ByteVariableCall paramB) : base(paramA, paramB)
        {
        }

        public Divide(Tuple<ByteVariableCall, ByteVariableCall> pars) : base(pars.Item1, pars.Item2) { }
        public override string ToString() => $"mov A, {((ByteVariableCall)_paramA).Variable}\nmov B, {((ByteVariableCall)_paramB).Variable}\ndiv AB";
    }
}