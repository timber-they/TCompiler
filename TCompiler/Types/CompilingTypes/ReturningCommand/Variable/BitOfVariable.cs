using System;
using System.Text;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    public class BitOfVariable : BitVariable 
        /* A strange variable due to the fact that the address isn't necessarily defined when finished compiling 
         * It can only get assigned because in every other situation it'd be an operation (BitOf)
         * It's still necessary though to change the value of relative bits of variables
         */                                             
    {
        public BitOfVariable(string baseaddress, ByteVariable bit, Label lEnd, Label lOn, Label lLoop2, Label lLoop1) : base(false, false, baseaddress, $"{baseaddress}.{bit}")
        {
            _bit = bit;
            _lEnd = lEnd;
            _lOn = lOn;
            _lLoop2 = lLoop2;
            _lLoop1 = lLoop1;
        }

        private readonly ByteVariable _bit;
        private readonly Label _lLoop1;
        private readonly Label _lLoop2;
        private readonly Label _lOn;
        private readonly Label _lEnd;
        public string RegisterLoop { private get; set; }

        /// <summary>
        /// moves the value from acc.0 into the bit of the address from the base variable
        /// </summary>
        /// <returns>
        /// The command to do so
        /// </returns>
        public override string ToString()
        {
            if(RegisterLoop == null)
                throw new Exception("You didn't define the register for the BitOf, Timo...");
            var sb = new StringBuilder();
            sb.AppendLine($"mov C, acc.0");                                     //I want to remember this bit
            sb.AppendLine($"mov AC, C");                                        //So I move it into the auxiliary Carry Flag
            sb.AppendLine("clr C");                                             //Because the carry must be cleared for the rotation

            sb.AppendLine($"jb AC, {_lOn}");                                    //I do different stuff when it's off or on. Her comes the off part: TODO change every sjmp

            sb.AppendLine($"mov A, #11111110b");                                //All the other bits must be on so I can later use anl without affecting other bits
            sb.AppendLine($"mov {RegisterLoop}, {_bit}");                       //I must rotate _bit times - this is a normal rotation, so that the off bit is at the correct position
            sb.AppendLine($"{_lLoop1}:");
            sb.AppendLine("rlc A");
            sb.AppendLine("addc A, #0");
            sb.AppendLine($"djnz {RegisterLoop}, {_lLoop1}");
            sb.AppendLine($"anl A, {Address}");                                 //Now only the selected bit (still in the accu) is changed
            
            sb.AppendLine($"jmp {_lEnd}");                                      //That was the off part


            sb.AppendLine($"{_lOn}:");                                          //And her comes the on part. It's similar to the off part but I don't use anl but orl, so the other bits may remain off

            sb.AppendLine("anl A, #1");                                         //only the zeroth bit is counting - now all the others are off
            sb.AppendLine($"mov {RegisterLoop}, {_bit}");
            sb.AppendLine($"{_lLoop2}:");
            sb.AppendLine("rlc A");
            sb.AppendLine("addc A, #0");
            sb.AppendLine($"djnz {RegisterLoop}, {_lLoop2}");

            sb.AppendLine($"orl A, {Address}");                                 //And here the above mentioned orl

            sb.AppendLine($"{_lEnd}:");                                         //That's it - now the solution is in the Accu

            
            sb.AppendLine($"mov {Address}, A");                                 //And now in the address
            return sb.ToString();
        }
    }
}