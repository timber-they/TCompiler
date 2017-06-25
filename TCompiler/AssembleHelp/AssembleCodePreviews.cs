#region

using System.Collections.Generic;
using System.Linq;
using System.Text;

using TCompiler.Enums;
using TCompiler.Types.CompilingTypes;
using TCompiler.Types.CompilingTypes.ReturningCommand;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion


//TODO implement Ac
namespace TCompiler.AssembleHelp
{
    /// <summary>
    ///     At least theoretically a few assembler code snippets that I can use
    /// </summary>
    public static class AssembleCodePreviews
    {
        /// <summary>
        ///     A code snippet that moves a single bit to a bitAddress. The destination bitAddress must be bit addressable
        /// </summary>
        /// <param name="destination">The destination bit</param>
        /// <param name="notLabel">The label to jump to if the bit is 0</param>
        /// <param name="endLabel">The label at the end (To jump over the other part)</param>
        /// <param name="bit">The bit that will be moved to the destination</param>
        /// <returns>The assembler code to execute as a string</returns>
        public static string MoveBitTo (BitVariable destination, Label notLabel, Label endLabel, BitVariable bit)
        {
            var sb = new StringBuilder ();

            if (!bit.IsConstant)
            {
                sb.AppendLine (bit.MoveThisIntoAcc0 ());
                sb.AppendLine ($"{Ac.JumpNotBit} 0E0h.0, {notLabel.DestinationName}");
                sb.AppendLine (destination.Set ());
                sb.AppendLine ($"{Ac.Jump} {endLabel.DestinationName}");
                sb.AppendLine (notLabel.LabelMark ());
                sb.AppendLine (destination.Clear ());
                sb.AppendLine (endLabel.LabelMark ());
            }
            else
                sb.AppendLine (bit.Value ? destination.Set () : destination.Clear ());

            return sb.ToString ();
        }

        /// <summary>
        ///     The part to execute before the main program
        /// </summary>
        /// <param name="externalLabel0">The name of the external interrupt 0 Interrupt Service Routine</param>
        /// <param name="externalLabel1">The name of the external interrupt 1 Interrupt Service Routine</param>
        /// <param name="timerCounterLabel0">The name of the timer/counter interrupt 0 Interrupt Service Routine</param>
        /// <param name="timerCounterLabel1">The name of the timer/counter interrupt 1 Interrupt Service Routine</param>
        /// <param name="isCounter0">Specifies wether the timer/counter 0 is a counter</param>
        /// <param name="isCounter1">Specifies wether the timer/counter 1 is a counter</param>
        /// <returns>The assembler code to execute as a string</returns>
        public static string Before (
            string externalLabel0, string externalLabel1, string timerCounterLabel0,
            string timerCounterLabel1, bool isCounter0, bool isCounter1)
        {
            var sb = new StringBuilder ();
            if (externalLabel0 == null &&
                externalLabel1 == null &&
                timerCounterLabel0 == null &&
                timerCounterLabel1 == null)
                return $"{sb}main:\n{Ac.Move} 081h, #127\n";
            sb.AppendLine ($"{Ac.LongJump} main");
            if (externalLabel0 != null)
            {
                sb.AppendLine ("org 03h");
                sb.AppendLine ($"call {externalLabel0}");
                sb.AppendLine ("reti");
            }
            if (externalLabel1 != null)
            {
                sb.AppendLine ("org 13h");
                sb.AppendLine ($"call {externalLabel1}");
                sb.AppendLine ("reti");
            }
            if (timerCounterLabel0 != null)
            {
                sb.AppendLine ("org 0Bh");
                sb.AppendLine ($"call {timerCounterLabel0}");
                sb.AppendLine ("reti");
            }
            if (timerCounterLabel1 != null)
            {
                sb.AppendLine ("org 1Bh");
                sb.AppendLine ($"call {timerCounterLabel1}");
                sb.AppendLine ("reti");
            }
            sb.AppendLine ("main:");
            if (externalLabel0 != null)
            {
                sb.AppendLine ($"{Ac.SetBit} 088h.0");
                sb.AppendLine ($"{Ac.Clear} 088h.1");
                sb.AppendLine ($"{Ac.SetBit} s0A8h.0");
            }
            if (externalLabel1 != null)
            {
                sb.AppendLine ($"{Ac.SetBit} 088h.2");
                sb.AppendLine ($"{Ac.Clear} 088h.3");
                sb.AppendLine ($"{Ac.SetBit} 0A8h.2");
            }
            sb.AppendLine ($"{Ac.Move} 089h, #0");
            if (timerCounterLabel0 != null)
            {
                sb.AppendLine (isCounter0 ? $"{Ac.Move} 089h, #00000101b" : $"{Ac.Move} 089h, #00000001b");
                sb.AppendLine ($"{Ac.SetBit} 088h.4");
                sb.AppendLine ($"{Ac.Clear} 088h.5");
                sb.AppendLine ($"{Ac.SetBit} 0A8h.1");
            }
            if (timerCounterLabel1 != null)
            {
                sb.AppendLine (isCounter1 ? $"{Ac.Or} 089h, #01010000b" : $"{Ac.Or} 089h, #00010000b");
                sb.AppendLine ($"{Ac.SetBit} 088h.6");
                sb.AppendLine ($"{Ac.Clear} 088h.7");
                sb.AppendLine ($"{Ac.SetBit} 0A8h.3");
            }

            sb.AppendLine ($"{Ac.SetBit} 0A8h.7");
            return sb.ToString ();
        }

        /// <summary>
        ///     The part to execute after the normal program
        /// </summary>
        /// <returns>The assembler code to execute as a string</returns>
        public static string After ()
        {
            var sb = new StringBuilder ();
            sb.AppendLine ($"{Ac.Jump} main");
            sb.AppendLine ($"{Ac.End}");

            return sb.ToString ();
        }

        /// <summary>
        ///     The part to execute before every command, if deactivateEa is true
        /// </summary>
        /// <param name="interruptExecutions">The enabled interrupt executions</param>
        /// <returns>The assembler code to execute as a string</returns>
        public static string BeforeCommand (IEnumerable<InterruptType> interruptExecutions)
            => interruptExecutions.Any () ? $"{Ac.Clear} 0A8h.7" : "";

        /// <summary>
        ///     The part to execute before every command, if activateEa is true
        /// </summary>
        /// <param name="interruptExecutions">The enabled interrupt executions</param>
        /// <returns>The assembler code to execute as a string</returns>
        public static string AfterCommand (IEnumerable<InterruptType> interruptExecutions)
            => interruptExecutions.Any () ? $"{Ac.SetBit} 0A8h.7" : "";

        /// <summary>
        ///     Moves paramA into the Accu and the paramB into the B register
        /// </summary>
        /// <param name="paramA">The parameter that will get moved into the Accu</param>
        /// <param name="paramB">The parameter that will get moved into the B register</param>
        /// <returns>The assembler code to execute as a string</returns>
        public static string MoveParametersIntoAb (ReturningCommand paramA, ReturningCommand paramB)
        {
            var sb = new StringBuilder ();
            sb.AppendLine (paramB.ToString ());
            sb.AppendLine ($"{Ac.Push} 0E0h");
            sb.AppendLine (paramA.ToString ());
            sb.AppendLine ($"{Ac.Pop} 0F0h");
            return sb.ToString ();
        }

        /// <summary>
        ///     Moves the Accu into the B register
        /// </summary>
        /// <returns>The assembler code to execute as a string</returns>
        public static string MoveAccuIntoB () => $"{Ac.Move} 0F0h, A";
    }
}