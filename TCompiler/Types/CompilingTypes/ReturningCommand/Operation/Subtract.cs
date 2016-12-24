using System;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation
{
    public class Subtract : TwoParameterOperation
    {
        public Subtract(ByteVariableCall paramA, ByteVariableCall paramB) : base(paramA, paramB)
        {
        }

        public Subtract(Tuple<ByteVariableCall, ByteVariableCall> pars) : base(pars.Item1, pars.Item2) { }
        public override string ToString() => $"clr C\nmov A, {((ByteVariableCall) _paramA).Variable}\nsubb A, {((ByteVariableCall)_paramB).Variable}";
    }
}