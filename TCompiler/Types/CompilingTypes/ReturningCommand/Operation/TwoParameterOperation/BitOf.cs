#region

using System;
using System.Text;

using TCompiler.AssembleHelp;
using TCompiler.Types.CompilerTypes;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation
{
    /// <summary>
    ///     Moves the in paramB specified bit from paramA to 0E0h.0<br />
    ///     Syntax:<br />
    ///     paramA.paramB
    /// </summary>
    public class BitOf : TwoParameterOperation
    {
        /// <summary>
        ///     The label at the end of the evaluation
        /// </summary>
        private readonly Label _lend;

        /// <summary>
        ///     The label to jump to to repeat the shifting
        /// </summary>
        private readonly Label _lLoop;

        private readonly Label _lNotZero;

        /// <summary>
        ///     The label to jump to when paramB is a constant value
        /// </summary>
        private readonly Label _lSet;

        /// <summary>
        ///     Initiates a new BitOf operation
        /// </summary>
        /// <param name="paramA">The byte to take the bit from</param>
        /// <param name="paramB">The bit-index of the byte</param>
        /// <param name="lend">The label at the end of the evaluation</param>
        /// <param name="lSet">The label to jump to when paramB is a constant value</param>
        /// <param name="lLoop">The label to jump to to repeat the shifting</param>
        /// <param name="lNotZero"></param>
        /// <param name="registerLoop">The register for the shifting loop. Make sure that it's only used here!</param>
        /// <param name="cLine">The original T code line</param>
        public BitOf(ReturningCommand paramA, ReturningCommand paramB, Label lend, Label lSet, Label lLoop,
            Label lNotZero, string registerLoop, CodeLine cLine)
            : base(paramA, paramB, cLine)
        {
            _lend = lend;
            _lSet = lSet;
            _lLoop = lLoop;
            _lNotZero = lNotZero;
            RegisterLoop = registerLoop;
        }

        /// <summary>
        ///     The register for the shifting loop
        /// </summary>
        private string RegisterLoop { get; }

        /// <summary>
        ///     Evaluates the stuff to execute in assembler to make a BitOf operation
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public override string ToString()
        {
            if (RegisterLoop == null)
                throw new Exception("You didn't define the register for the BitOf, Timo...");
            var sb = new StringBuilder();
            if ((ParamB as ByteVariableCall)?.ByteVariable?.IsConstant == true)
            {
                sb.AppendLine($"{ParamA}");
                sb.AppendLine($"jb 0E0h.{((ByteVariableCall) ParamB).ByteVariable.Value}, {_lSet.DestinationName}");
                sb.AppendLine($"{Ac.Clear} 0E0h.0");
                sb.AppendLine($"{Ac.Jump} {_lend.DestinationName}");
                sb.AppendLine(_lSet.LabelMark());
                sb.AppendLine($"{Ac.SetBit} 0E0h.0");
            }
            else
            {
                sb.AppendLine($"{Ac.Clear} C");
                sb.AppendLine($"{ParamB}");
                sb.AppendLine($"{Ac.Move} {RegisterLoop}, A");
                sb.AppendLine($"{ParamA}");
                sb.AppendLine($"cjne {RegisterLoop}, #0, {_lNotZero.DestinationName}");
                sb.AppendLine($"{Ac.Jump} {_lend.DestinationName}");
                sb.AppendLine(_lNotZero.LabelMark());
                sb.AppendLine(_lLoop.LabelMark());
                sb.AppendLine("rrc A");
                sb.AppendLine($"{Ac.Add}c A, #0");
                sb.AppendLine($"djnz {RegisterLoop}, {_lLoop.DestinationName}");
            }
            sb.AppendLine(_lend.LabelMark());
            return sb.ToString();
        }
    }
}