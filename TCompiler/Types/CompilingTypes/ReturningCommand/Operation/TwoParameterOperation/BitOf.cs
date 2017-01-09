#region

using System;
using System.Text;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation
{
    public class BitOf : TwoParameterOperation
    {
        private readonly Label _l1;
        private readonly Label _lend;
        private readonly Label _lLoop;

        public BitOf(VariableCall paramA, VariableCall paramB, Label lend, Label l1, Label lLoop) : base(paramA, paramB)
        {
            _lend = lend;
            _l1 = l1;
            _lLoop = lLoop;
        }

        public string RegisterLoop { private get; set; }

        public override string ToString()
        {
            if (RegisterLoop == null)
                throw new Exception("You didn't define the register for the BitOf, Timo...");
            var sb = new StringBuilder();
            if (((ByteVariableCall) ParamB).ByteVariable.IsConstant)
            {
                sb.AppendLine($"{ParamA}");
                sb.AppendLine($"jb acc.{((ByteVariableCall) ParamB).ByteVariable.Value}, {_l1.DestinationName}");
            }
            else
            {
                sb.AppendLine("clr C");
                sb.AppendLine($"{ParamB}");
                sb.AppendLine($"mov {RegisterLoop}, A");
                sb.AppendLine($"{ParamA}");
                sb.AppendLine(_lLoop.LabelMark());
                sb.AppendLine("rrc A");
                sb.AppendLine("addc A, #0");
                sb.AppendLine($"djnz {RegisterLoop}, {_lLoop.DestinationName}");
                sb.AppendLine($"jb acc.0, {_l1.DestinationName}");
            }
            sb.AppendLine("clr acc.0");
            sb.AppendLine($"jmp {_lend.DestinationName}");
            sb.AppendLine(_l1.LabelMark());
            sb.AppendLine("setb acc.0");
            sb.AppendLine(_lend.LabelMark());
            return sb.ToString();
        }
    }
}