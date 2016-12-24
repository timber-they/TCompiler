using System;
using System.Text;
using TCompiler.Compiling;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation.Compare
{
    public class Equal : Compare
    {
        public Equal(ByteVariableCall paramA, ByteVariableCall paramB) : base(paramA, paramB)
        {
        }

        public Equal(Tuple<ByteVariableCall, ByteVariableCall> pars) : base(pars)
        {
        }

        public override string ToString()
        {
            var notequal = ParseToAssembler.Label1;
            var end = ParseToAssembler.Label1;
            var a = (ByteVariableCall)_paramA;
            var b = (ByteVariableCall)_paramB;

            var sb = new StringBuilder();
            sb.AppendLine($"mov A, {a.Variable}");
            sb.AppendLine($"cjne A, {b.Variable}, {notequal}");
            sb.AppendLine("clr acc.0");
            sb.AppendLine($"jmp {end}");
            sb.AppendLine($"{notequal}: setb acc.0");
            sb.AppendLine($"{end}:");

            return sb.ToString();
        }
    }
}