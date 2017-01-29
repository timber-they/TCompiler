#region

using System.Text;
using TCompiler.AssembleHelp;
using TCompiler.Settings;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation.Compare
{
    /// <summary>
    ///     Indicates wether parmA and paramB are unequal<br />
    ///     Syntax:<br />
    ///     paramA != paramB
    /// </summary>
    public class UnEqual : Compare
    {
        /// <summary>
        ///     Initializes a new Unequal comparison
        /// </summary>
        /// <param name="paramA">The first parameter to compare</param>
        /// <param name="paramB">The second parameter to compare</param>
        public UnEqual(ReturningCommand paramA, ReturningCommand paramB) : base(paramA, paramB)
        {
        }

        /// <summary>
        ///     Evaluates the stuff to execute in assembler to make an unequal comparison
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public override string ToString()
        {
            var notequal = GlobalProperties.Label;
            var end = GlobalProperties.Label;

            var sb = new StringBuilder();
            sb.AppendLine(AssembleCodePreviews.MoveParametersIntoAb(ParamA, ParamB));
            sb.AppendLine($"cjne A, 0F0h, {notequal.DestinationName}");
            sb.AppendLine("clr 224.0");
            sb.AppendLine($"jmp {end.DestinationName}");
            sb.AppendLine(notequal.LabelMark());
            sb.AppendLine("setb 224.0");
            sb.AppendLine(end.LabelMark());

            return sb.ToString();
        }
    }
}