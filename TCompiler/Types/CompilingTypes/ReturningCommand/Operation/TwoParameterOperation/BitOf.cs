using System;
using System.Text;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation
{
    public class BitOf : TwoParameterOperation
    {
        private readonly Label _l1;
        private readonly Label _lend;
        private readonly Label _lLoop;
        public string RegisterLoop { private get; set; }

        public BitOf(VariableCall paramA, VariableCall paramB, Label lend, Label l1, Label lLoop) : base(paramA, paramB)
        {
            _lend = lend;
            _l1 = l1;
            _lLoop = lLoop;
        }

        public override string ToString()
        {
            if (RegisterLoop == null)
                throw new Exception("You didn't define the register for the BitOf, Timo...");
            var sb = new StringBuilder();
            if (((ByteVariableCall) ParamB).Variable.IsConstant)
            {
                sb.AppendLine($"{ParamA}");
                sb.AppendLine($"jb acc.{((ByteVariableCall) ParamB).Variable.Value}, {_l1}");
            }
            else
            {
                sb.AppendLine("clr C");
                sb.AppendLine($"{ParamB}");
                sb.AppendLine($"mov {RegisterLoop}, A");
                sb.AppendLine($"{ParamA}");
                sb.AppendLine($"{_lLoop}:");
                sb.AppendLine("rrc A");
                sb.AppendLine("addc A, #0");
                sb.AppendLine($"djnz {RegisterLoop}, {_lLoop}");
                sb.AppendLine($"jb acc.0, {_l1}");
            }
            sb.AppendLine("clr acc.0");
            sb.AppendLine($"jmp {_lend}");
            sb.AppendLine($"{_l1}:");
            sb.AppendLine("setb acc.0");
            sb.AppendLine($"{_lend}:");
            return sb.ToString();
        }
    }
}