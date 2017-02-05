#region

using System.Text;
using TCompiler.Types.CompilerTypes;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation
{
    /// <summary>
    ///     A left shift operator, that shifts paramA by paramB digits to the left.<br />
    ///     Syntax:<br />
    ///     paramA &#60;&#60; paramB
    /// </summary>
    public class ShiftLeft : TwoParameterOperation
    {
        /// <summary>
        ///     The label to jump to in the shifting loop
        /// </summary>
        private readonly Label _label;

        /// <summary>
        ///     The register that is decreased in the shifting loop
        /// </summary>
        private readonly string _register;

        /// <summary>
        ///     Initializes a new ShiftLeft operation
        /// </summary>
        /// <param name="paramA">The parameter that is being shifted</param>
        /// <param name="paramB">Indicates by how many digits the first parameter is shifted to the left</param>
        /// <param name="register">The register that is decreased in the shifting loop</param>
        /// <param name="label">The label to jump to in the shifting loop</param>
        /// <param name="cLine">The original T code line</param>
        public ShiftLeft(ReturningCommand paramA, ReturningCommand paramB, string register, Label label, CodeLine cLine)
            : base(paramA, paramB, cLine)
        {
            _register = register;
            _label = label;
        }

        /// <summary>
        ///     Evaluates the stuff to execute in assembler to make a ShiftLeft operation
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("clr C");
            sb.AppendLine($"{ParamB}");
            sb.AppendLine($"mov {_register}, A");
            sb.AppendLine($"{ParamA}");
            sb.AppendLine($"{_label.LabelMark()}");
            sb.AppendLine("rlc A");
            sb.AppendLine("addc A, #0");
            sb.AppendLine($"djnz {_register}, {_label.DestinationName}");
            return sb.ToString();
        }
    }
}