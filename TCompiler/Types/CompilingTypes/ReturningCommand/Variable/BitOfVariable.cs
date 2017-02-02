#region

using System;
using System.Text;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    /// <summary>
    ///     A strange variable due to the fact that the address isn't necessarily defined when finished compiling<br />
    ///     It can only get assigned, because in every other situation it'd be an operation (BitOf)<br />
    ///     It's still necessary though to change the value of relative bits of variables<br />
    ///     Syntax:<br />
    ///     baseaddress.bit
    /// </summary>
    public class BitOfVariable : BitVariable
    {
        /// <summary>
        ///     The bit-index of the bit. Can be undefined by compile-time
        /// </summary>
        private readonly ByteVariable _bit;

        /// <summary>
        ///     The label at the end of the evaluation
        /// </summary>
        private readonly Label _lEnd;

        /// <summary>
        ///     The first endLabel for the end of the shifting loop
        /// </summary>
        private readonly Label _lEnd0;

        /// <summary>
        ///     The second endLabel for the end of the shifting loop
        /// </summary>
        private readonly Label _lEnd1;

        /// <summary>
        ///     The first loop label (for shifting)
        /// </summary>
        private readonly Label _lLoop0;

        /// <summary>
        ///     The second loop label (for shifting)
        /// </summary>
        private readonly Label _lLoop1;

        /// <summary>
        ///     The label to jump to when the bit that has the value to assign is true
        /// </summary>
        private readonly Label _lOn;

        /// <summary>
        ///     The first label to jump to when no rotation is required
        /// </summary>
        private readonly Label _lNotZero0;

        /// <summary>
        ///     The second label to jump to when no rotation is required
        /// </summary>
        private readonly Label _lNotZero1;

        /// <summary>
        ///     Initializes a new BitOfVariable
        /// </summary>
        /// <param name="baseaddress">The address where the bit is from</param>
        /// <param name="bit">The bit-index of the bit. Can be undefined by compile-time</param>
        /// <param name="lOn">The label to jump to when the bit that has the value to assign (0E0h.0) is true</param>
        /// <param name="lLoop0">The first loop label (for shifting)</param>
        /// <param name="lLoop1">The second loop label (for shifting)</param>
        /// <param name="lNotZero0">The first label to jump to when no rotation is required</param>
        /// <param name="lNotZero1">the second label to jump to when no rotation is required</param>
        /// <param name="lEnd0">The first endLabel for the end of the shifting loop</param>
        /// <param name="lEnd1">The second endLabel for the end of the shifting loop</param>
        /// <param name="lEnd">The label at the end of the evaluation</param>
        public BitOfVariable(Address baseaddress, ByteVariable bit, Label lOn, Label lLoop0, Label lLoop1, Label lNotZero0,
            Label lNotZero1, Label lEnd0, Label lEnd1, Label lEnd)
            : base(false, false, baseaddress, $"{baseaddress}.{bit.Address}")
        {
            _bit = bit;
            _lEnd = lEnd;
            _lOn = lOn;
            _lLoop1 = lLoop1;
            _lLoop0 = lLoop0;
            _lNotZero0 = lNotZero0;
            _lNotZero1 = lNotZero1;
            _lEnd1 = lEnd1;
            _lEnd0 = lEnd0;
        }

        /// <summary>
        ///     The register for the loop - must be assigned before MoveAcc0IntoThis is called
        /// </summary>
        public string RegisterLoop { private get; set; }

        /// <summary>
        ///     Moves the value from 0E0h.0 into the bit of the address from the base variable
        /// </summary>
        /// <returns>
        ///     The assembler code as a string
        /// </returns>
        public override string MoveAcc0IntoThis()
        {
            if (Address.IsBitAddressableInSpecialFunctionRegister() && _bit.IsConstant)
                //If it's in the sfr and the bitof is constant you can directly address it
                return $"jb 0E0h.0, {_lOn.DestinationName}\n" +
                       $"clr {Address}.{_bit.Value}\n" +
                       $"jmp {_lEnd.DestinationName}\n" +
                       $"{_lOn.LabelMark()}\n" +
                       $"setb {Address}.{_bit.Value}\n" +
                       _lEnd.LabelMark();

            if (RegisterLoop == null)
                throw new Exception("You didn't define the register for the BitOf, Timo...");
            var sb = new StringBuilder();
            sb.AppendLine("mov C, 0E0h.0"); //I want to remember this bit
            sb.AppendLine("mov 208.6, C"); //So I move it into the auxiliary Carry Flag
            sb.AppendLine("clr C"); //Because the carry must be cleared for the rotation

            sb.AppendLine($"jb 208.6, {_lOn.DestinationName}");
            //I do different stuff when it's off or on. Her comes the off part:

            sb.AppendLine("mov A, #11111110b");
            //All the other bits must be on so I can later use anl without affecting other bits

            if (!Address.IsInExtendedMemory)
                sb.AppendLine($"mov {RegisterLoop}, {_bit.Address}");
            else
            {
                sb.AppendLine(Address.MoveThisIntoDataPointer());
                sb.AppendLine($"movx {RegisterLoop}, @dptr");
            }

            sb.AppendLine($"cjne {RegisterLoop}, #0, {_lNotZero0.DestinationName}"); //Don't rotate when it's zero!
            sb.AppendLine($"jmp {_lEnd0.DestinationName}");
            sb.AppendLine(_lNotZero0.LabelMark());

            //I must rotate _bit times - this is a normal rotation, so that the off bit is at the correct position
            sb.AppendLine(_lLoop0.LabelMark());
            sb.AppendLine("rlc A");
            sb.AppendLine("addc A, #0");
            sb.AppendLine($"djnz {RegisterLoop}, {_lLoop0.DestinationName}");

            sb.AppendLine(_lEnd0.LabelMark());

            if (!Address.IsInExtendedMemory)
                sb.AppendLine($"anl A, {Address}"); //Now only the selected bit (still in the accu) is changed
            else
            {
                sb.AppendLine(Address.MoveThisIntoDataPointer());
                sb.AppendLine("mov 0F0h, A");
                sb.AppendLine("movx A, @dptr");
                sb.AppendLine("anl A, 0F0h");
            }

            sb.AppendLine($"jmp {_lEnd.DestinationName}"); //That was the off part


            sb.AppendLine(_lOn.LabelMark());
            //And her comes the on part. It's similar to the off part but I don't use anl but orl, so the other bits may remain off

            sb.AppendLine("anl A, #1"); //only the zeroth bit is counting - now all the others are off

            if (!Address.IsInExtendedMemory)
                sb.AppendLine($"mov {RegisterLoop}, {_bit.Address}");
            else
            {
                sb.AppendLine(_bit.Address.MoveThisIntoDataPointer());
                sb.AppendLine($"movx {RegisterLoop}, @dptr");
            }

            sb.AppendLine($"cjne {RegisterLoop}, #0, {_lNotZero1.DestinationName}"); //Again - don't rotate when it's zero!
            sb.AppendLine($"jmp {_lEnd1.DestinationName}");
            sb.AppendLine(_lNotZero1.LabelMark());
            sb.AppendLine(_lLoop1.LabelMark());
            sb.AppendLine("rlc A");
            sb.AppendLine("addc A, #0");
            sb.AppendLine($"djnz {RegisterLoop}, {_lLoop1.DestinationName}");

            sb.AppendLine(_lEnd1.LabelMark());

            if (!Address.IsInExtendedMemory)
                sb.AppendLine($"orl A, {Address}"); //And here the above mentioned orl
            else
            {
                sb.AppendLine(Address.MoveThisIntoDataPointer());
                sb.AppendLine("mov 0F0h, A");
                sb.AppendLine("movx A, @dptr");
                sb.AppendLine("orl A, 0F0h");
            }

            sb.AppendLine(_lEnd.LabelMark()); //That's it - now the solution is in the Accu

            if (!Address.IsInExtendedMemory)
                sb.AppendLine($"mov {Address}, A"); //And now in the address
            else
            {
                sb.AppendLine(Address.MoveThisIntoDataPointer());
                sb.AppendLine("movx @dptr, A");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Noves another variable into this variable
        /// </summary>
        /// <param name="variable">The other variable</param>
        /// <returns>The assembler code to execute as a string</returns>
        public override string MoveVariableIntoThis(VariableCall variable) => $"{variable}\n{MoveAcc0IntoThis()}";
    }
}