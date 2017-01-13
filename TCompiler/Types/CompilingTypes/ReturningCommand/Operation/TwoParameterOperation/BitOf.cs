#region

using System;
using System.Text;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation
{
    /// <summary>
    /// Moves the in paramB specified bit from paramA to acc.0<br/>
    /// Syntax:<br/>
    /// paramA.paramB
    /// </summary>
    public class BitOf : TwoParameterOperation
    {
        /// <summary>
        /// The label to jump to when paramB is a constant value
        /// </summary>
        private readonly Label _lConstant;
        /// <summary>
        /// The label at the end of the evaluation
        /// </summary>
        private readonly Label _lend;
        /// <summary>
        /// The label to jump to to repeat the shifting
        /// </summary>
        private readonly Label _lLoop;

        /// <summary>
        /// Initiates a new BitOf operation
        /// </summary>
        /// <param name="paramA">The byte to take the bit from</param>
        /// <param name="paramB">The bit-index of the byte</param>
        /// <param name="lend">The label at the end of the evaluation</param>
        /// <param name="lConstant">The label to jump to when paramB is a constant value</param>
        /// <param name="lLoop">The label to jump to to repeat the shifting</param>
        public BitOf(VariableCall paramA, VariableCall paramB, Label lend, Label lConstant, Label lLoop) : base(paramA, paramB)
        {
            _lend = lend;
            _lConstant = lConstant;
            _lLoop = lLoop;
        }

        /// <summary>
        /// The register for the shifting loop
        /// </summary>
        public string RegisterLoop { private get; set; }

        /// <summary>
        /// Evaluates the stuff to execute in assembler to make a BitOf operation
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public override string ToString()
        {
            if (RegisterLoop == null)
                throw new Exception("You didn't define the register for the BitOf, Timo...");
            var sb = new StringBuilder();
            if (((ByteVariableCall) ParamB).ByteVariable.IsConstant)
            {
                sb.AppendLine($"{ParamA}");
                sb.AppendLine($"jb acc.{((ByteVariableCall) ParamB).ByteVariable.Value}, {_lConstant.DestinationName}");
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
                sb.AppendLine($"jb acc.0, {_lConstant.DestinationName}");
            }
            sb.AppendLine("clr acc.0");
            sb.AppendLine($"jmp {_lend.DestinationName}");
            sb.AppendLine(_lConstant.LabelMark());
            sb.AppendLine("setb acc.0");
            sb.AppendLine(_lend.LabelMark());
            return sb.ToString();
        }
    }
}