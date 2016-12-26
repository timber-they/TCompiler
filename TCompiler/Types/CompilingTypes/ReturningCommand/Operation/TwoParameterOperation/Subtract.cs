using System;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation
{
    public class Subtract : TwoParameterOperation
    {
        public Subtract(ByteVariableCall paramA, ByteVariableCall paramB) : base(paramA, paramB)
        {
        }

        public Subtract(Tuple<ByteVariableCall, ByteVariableCall> pars) : base(pars.Item1, pars.Item2) { }
        public override string ToString() => $"{_paramA}\nclr C\nsubb A, {((ByteVariableCall)_paramB).Variable}";
    }
}