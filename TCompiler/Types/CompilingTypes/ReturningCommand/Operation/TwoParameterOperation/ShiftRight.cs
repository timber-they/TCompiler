using System.Text;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation
{
    /// <summary>
    ///     A right shift operator, that shifts paramA by paramB difits to the right.<br />
    ///     Syntax:<br />
    ///     paramA >> paramB
    /// </summary>
    public class ShiftRight : TwoParameterOperation
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
        ///     Initializes a new ShiftRight operation
        /// </summary>
        /// <param name="paramA">The parameter that is being shifted</param>
        /// <param name="paramB">Indicates by how many digits the first parameter is shifted to the right</param>
        /// <param name="register">The register that is decreased in the shifting loop</param>
        /// <param name="label">The label to jump to in the shifting loop</param>
        public ShiftRight(ReturningCommand paramA, ReturningCommand paramB, string register, Label label) : base(paramA, paramB)
        {
            _register = register;
            _label = label;
        }

        /// <summary>
        ///     Evaluates the stuff to execute in assembler to make a ShiftRight operation
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("clr C\n");
            sb.AppendLine($"{ParamB}\n");
            sb.AppendLine($"mov {_register}, A\n");
            sb.AppendLine($"{ParamA}\n");
            sb.AppendLine($"{_label.LabelMark()}\n");
            sb.AppendLine("rrc A\n");
            sb.AppendLine("addc A, #0\n");
            sb.AppendLine($"djnz {_register}, {_label.DestinationName}");
            return sb.ToString();
        }
    }
}