using System;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation
{
    public class Modulo : TwoParameterOperation
    {
        public Modulo(ByteVariableCall paramA, ByteVariableCall paramB) : base(paramA, paramB)
        {
        }

        public Modulo(Tuple<ByteVariableCall, ByteVariableCall> pars) : base(pars.Item1, pars.Item2) { }
        public override string ToString() => $"mov A, {((ByteVariableCall)_paramA).Variable}\nmov B, {((ByteVariableCall)_paramB).Variable}\ndiv AB\nxch A, B";
    }
}