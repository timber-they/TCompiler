using System;
using System.Text;
using TCompiler.Compiling;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.Compare
{
    public class Bigger : Compare
    {
        public Bigger(ByteVariableCall paramA, ByteVariableCall paramB) : base(paramA, paramB)
        {
        }

        public Bigger(Tuple<ByteVariableCall, ByteVariableCall> pars) : base(pars)
        {
        }

        public override string ToString()
        {
            var notequal = ParseToAssembler.Label1;
            var end = ParseToAssembler.Label1;
            var bigger = ParseToAssembler.Label1;
            var a = (ByteVariableCall)_paramA;
            var b = (ByteVariableCall)_paramB;

            var sb = new StringBuilder();
            sb.AppendLine($"mov A, {a.Variable}");
            sb.AppendLine($"cjne A, {b.Variable}, {notequal}");
            sb.AppendLine("clr acc.0");
            sb.AppendLine($"jmp {end}");
            sb.AppendLine($"{notequal}: jnb C, {bigger}");
            sb.AppendLine("clr acc.0");
            sb.AppendLine($"jmp {end}");
            sb.AppendLine($"{bigger}: setb acc.0");
            sb.AppendLine($"{end}:");

            return sb.ToString();
        }
    }
}