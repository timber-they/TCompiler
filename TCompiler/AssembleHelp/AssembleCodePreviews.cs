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
        /// <param name="notlabel">The label to jump to if the bit is 0</param>
        /// <param name="endLabel">The label at the end (To jump over the other part)</param>
        /// <param name="bit">The bit that will be moved to the Accu</param>
        public static string MoveBitToAccu(Label notlabel, Label endLabel, BitVariableCall bit)
            => MoveBitTo(new Bool(false, "acc.0", "a0"), notlabel, endLabel, bit.Variable);

        public static string MoveBitTo(BitVariable destination, Label notLabel, Label endLabel, BitVariable bit)
        {
            var sb = new StringBuilder();

            if (!bit.IsConstant)
            {
                sb.AppendLine($"jnb {bit.Address}, {notLabel.DestinationName}");
                sb.AppendLine($"setb {destination.Address}");
                sb.AppendLine($"jmp {endLabel.DestinationName}");
                sb.AppendLine(notLabel.LabelMark());
                sb.AppendLine($"clr {destination.Address}");
                sb.AppendLine(endLabel.LabelMark());
            }
            else
                sb.AppendLine(bit.Value ? $"setb {destination.Address}" : $"clr {destination.Address}");

            return sb.ToString();
        }
    }
}