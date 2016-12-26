using System;
using System.Text;
using TCompiler.Compiling;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation.Compare
{
    public class Smaller : Compare
    {
        public Smaller(ByteVariableCall paramA, ByteVariableCall paramB) : base(paramA, paramB)
        {
        }

        public Smaller(Tuple<ByteVariableCall, ByteVariableCall> pars) : base(pars)
        {
        }

        public override string ToString()
        {
            var notequal = ParseToAssembler.Label;
            var end = ParseToAssembler.Label;
            var bigger = ParseToAssembler.Label;
            var a = (ByteVariableCall) _paramA;
            var b = (ByteVariableCall) _paramB;

            var sb = new StringBuilder();
            sb.AppendLine($"mov A, {a.Variable}");
            sb.AppendLine($"cjne A, {b.Variable}, {notequal}");
            sb.AppendLine("clr acc.0");
            sb.AppendLine($"jmp {end}");
            sb.AppendLine($"{notequal}: jnb C, {bigger}");
            sb.AppendLine("setb acc.0");
            sb.AppendLine($"jmp {end}");
            sb.AppendLine($"{bigger}: clr acc.0");
            sb.AppendLine($"{end}:");

            return sb.ToString();
        }
    }
}