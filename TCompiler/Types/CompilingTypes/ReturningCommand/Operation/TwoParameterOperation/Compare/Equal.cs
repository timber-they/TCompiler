#region

using System.Text;
using TCompiler.AssembleHelp;
using TCompiler.Compiling;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation.Compare
{
    /// <summary>
    ///     Indicates wether two integral values are equal<br />
    ///     Syntax:<br />
    ///     paramA = paramB
    /// </summary>
    public class Equal : Compare
    {
        /// <summary>
        ///     Initializes a new Equal operation
        /// </summary>
        /// <param name="paramA">The first parameter to compare</param>
        /// <param name="paramB">The second parameter to compare</param>
        public Equal(ReturningCommand paramA, ReturningCommand paramB) : base(paramA, paramB)
        {
        }

        /// <summary>
        ///     Evaluates the stuff to execute in assembler to make an equal comparison
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public override string ToString()
        {
            var notequal = ParseToAssembler.Label;
            var end = ParseToAssembler.Label;

            var sb = new StringBuilder();
            sb.AppendLine(AssembleCodePreviews.MoveParametersIntoAb(ParamA, ParamB));
            sb.AppendLine($"cjne A, B, {notequal.DestinationName}");
            sb.AppendLine("setb 224.0");
            sb.AppendLine($"jmp {end.DestinationName}");
            sb.AppendLine(notequal.LabelMark());
            sb.AppendLine("clr 224.0");
            sb.AppendLine(end.LabelMark());

            return sb.ToString();
        }
    }
}