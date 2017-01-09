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

        /// <summary>
        /// A code snippet that moves a single bit to a bitAddress. The destination bitAddress must be bit addressable
        /// </summary>
        /// <param name="destination">The destination bit</param>
        /// <param name="notLabel">The label to jump to if the bit is 0</param>
        /// <param name="endLabel">The label at the end (To jump over the other part)</param>
        /// <param name="bit">The bit that will be moved to the destination</param>
        /// <returns>The assembler code to execute as a string</returns>
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

        /// <summary>
        /// The part to execute before the main program
        /// </summary>
        /// <param name="ext0">The name of the external interrupt 0 Interrupt Service Routine</param>
        /// <param name="ext1">The name of the external interrupt 1 Interrupt Service Routine</param>
        /// <returns>The assembler code to execute as a string</returns>
        public static string Before(string ext0, string ext1)
        {
            var sb = new StringBuilder("include reg8051.inc\n");
            if (ext0 == null && ext1 == null)
                return $"{sb}main:\nmov SP, #127\n";
            sb.AppendLine("ljmp main");
            if (ext0 != null)
            {
                sb.AppendLine("org 03h");
                sb.AppendLine($"call {ext0}");
                sb.AppendLine("reti");
            }
            if (ext1 != null)
            {
                sb.AppendLine("org 13h");
                sb.AppendLine($"call {ext1}");
                sb.AppendLine("reti");
            }
            sb.AppendLine("main:");
            if (ext0 != null)
            {
                sb.AppendLine("setb IT0");
                sb.AppendLine("clr IE0");
                sb.AppendLine("setb EX0");
            }
            if (ext1 != null)
            {
                sb.AppendLine("setb IT1");
                sb.AppendLine("clr IE1");
                sb.AppendLine("setb EX1");
            }
            sb.AppendLine("setb EA");
            sb.AppendLine("mov SP, #127");
            return sb.ToString();
        }

        /// <summary>
        /// The part to execute after the normal program
        /// </summary>
        /// <returns>The assembler code to execute as a string</returns>
        public static string After()
        {
            var sb = new StringBuilder();
            sb.AppendLine("jmp main");
            sb.AppendLine("end");

            return sb.ToString();
        }

        public static string BeforeCommand(bool e0E, bool e1E)
            => !e0E && !e1E ? "" : "clr EA";
        public static string AfterCommand(bool e0E, bool e1E)
            => !e0E && !e1E ? "" : "setb EA";
    }
}