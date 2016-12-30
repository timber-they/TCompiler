using System.Globalization;
using System.Text;
using TCompiler.Types.CompilingTypes;
using TCompiler.Types.CompilingTypes.ReturningCommand;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.AssembleHelp
{
    public static class AssembleCodePreviews
    {
        public static string MoveBitToAccu(Label notlabel, Label endLabel, BitVariableCall bit)
        {
            var sb = new StringBuilder();

            if (!bit.Variable.IsConstant)
            {
                sb.AppendLine($"jnb {bit.Variable.Name}, {notlabel}");
                sb.AppendLine("setb acc.0");
                sb.AppendLine($"jmp {endLabel}");
                sb.AppendLine($"{notlabel}:");
                sb.AppendLine("clr acc.0");
                sb.AppendLine($"{endLabel}:");
            }
            else
                sb.AppendLine(bit.Variable.Value ? "setb acc.0" : "clr acc.0");

            return sb.ToString();
        }
    }
}