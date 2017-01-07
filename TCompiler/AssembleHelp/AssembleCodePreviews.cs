#region

using System.Text;
using TCompiler.Types.CompilingTypes;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

namespace TCompiler.AssembleHelp
{
    /// <summary>
    /// At least theoretically a few assembler code snippets that I can use
    /// </summary>
    public static class AssembleCodePreviews
    {
        /// <summary>
        /// A code snippet that moves a single bit to the first bit of the Accu
        /// </summary>
        /// <returns>The string that has to get executed in assembler</returns>
        public static string MoveBitToAccu(Label notlabel, Label endLabel, BitVariableCall bit)
        {
            var sb = new StringBuilder();

            if (!bit.Variable.IsConstant)
            {
                sb.AppendLine($"jnb {bit.Variable}, {notlabel}");
                sb.AppendLine("setb acc.0");
                sb.AppendLine($"jmp {endLabel}");
                sb.AppendLine(notlabel.LabelMark());
                sb.AppendLine("clr acc.0");
                sb.AppendLine(endLabel.LabelMark());
            }
            else
                sb.AppendLine(bit.Variable.Value ? "setb acc.0" : "clr acc.0");

            return sb.ToString();
        }
    }
}