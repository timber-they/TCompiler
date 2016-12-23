using System.Globalization;
using System.Text;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.AssembleHelp
{
    public static class AssembleCodePreviews
    {
        public static string MoveBitToAccu(string label, BitVariable bit)
        {
            var sb = new StringBuilder();

            var notLabel = label;
            var endLabel = label;

            if (!bit.IsConstant)
            {
                sb.AppendLine($"jnb {bit.Name}, {label}");
                sb.AppendLine("setb acc.0");
                sb.AppendLine($"jmp {endLabel}");
                sb.AppendLine($"{notLabel}:");
                sb.AppendLine("clr acc.0");
                sb.AppendLine($"{endLabel}:");
            }
            else
                sb.AppendLine(bit.Value ? "setb acc.0" : "clr acc.0");

            return sb.ToString();
        }
    }
}