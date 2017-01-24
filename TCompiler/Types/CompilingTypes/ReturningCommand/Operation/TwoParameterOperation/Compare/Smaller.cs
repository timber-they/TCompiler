#region

using System.Text;
using TCompiler.Compiling;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation.Compare
{
    /// <summary>
    ///     Indicates wether paramA is smaller than paramB<br />
    ///     Syntax:<br />
    ///     paramA &#60; paramB
    /// </summary>
    public class Smaller : Compare
    {
        /// <summary>
        ///     Initializes a new Smaller operation
        /// </summary>
        /// <param name="paramA">The smaller parameter</param>
        /// <param name="paramB">The bigger or equal parameter</param>
        public Smaller(ReturningCommand paramA, ByteVariableCall paramB) : base(paramA, paramB)
        {
        }

        /// <summary>
        ///     Evaluates the stuff to execute in assembler to make a smaller comparison
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public override string ToString()
        {
            var notequal = ParseToAssembler.Label;
            var end = ParseToAssembler.Label;
            var bigger = ParseToAssembler.Label;
            var b = (ByteVariableCall) ParamB;

            var sb = new StringBuilder();
            sb.AppendLine(ParamA.ToString());
            sb.AppendLine($"cjne A, {b.ByteVariable}, {notequal.DestinationName}");
            sb.AppendLine("clr 224.0");
            sb.AppendLine($"jmp {end.DestinationName}");
            sb.AppendLine(notequal.LabelMark());
            sb.AppendLine($"jnc {bigger.DestinationName}");
            sb.AppendLine("setb 224.0");
            sb.AppendLine($"jmp {end.DestinationName}");
            sb.AppendLine(bigger.LabelMark());
            sb.AppendLine("clr 224.0");
            sb.AppendLine(end.LabelMark());

            return sb.ToString();
        }
    }
}