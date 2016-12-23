using System;
using System.Text;
using TCompiler.Compiling;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.Compare
{
    public class Equal : Compare
    {
        public Equal(Variable.Variable paramA, Variable.Variable paramB) : base(paramA, paramB)
        {
        }

        public Equal(Tuple<Variable.Variable, Variable.Variable> pars) : base(pars)
        {
        }

        public override string ToString()
        {
            var notequal = ParseToAssembler.Label1;
            var end = ParseToAssembler.Label1;

            var sb = new StringBuilder();
            sb.AppendLine($"mov A, {_paramA}");
            sb.AppendLine($"cjne A, {_paramB}, {notequal}");
            sb.AppendLine("clr acc.0");
            sb.AppendLine($"jmp {end}");
            sb.AppendLine($"{notequal}: setb acc.0");
            sb.AppendLine($"{end}:");

            return sb.ToString();
        }
    }
}