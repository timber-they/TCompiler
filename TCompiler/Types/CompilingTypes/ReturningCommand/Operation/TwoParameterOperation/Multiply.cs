#region

using System.Text;
using TCompiler.AssembleHelp;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation
{
    /// <summary>
    ///     Evaluates paramA multiplied by paramB<br />
    ///     Syntax<br />
    ///     paramA * paramB
    /// </summary>
    public class Multiply : TwoParameterOperation
    {
        /// <summary>
        ///     Initializes a new multiply operation
        /// </summary>
        /// <param name="paramA">The first parameter to multiply</param>
        /// <param name="paramB">The second parameter to multiply the first with</param>
        public Multiply(ReturningCommand paramA, ReturningCommand paramB) : base(paramA, paramB)
        {
        }

        /// <summary>
        ///     Evaluates the stuff to execute in assembler to make a multiply operation
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(AssembleCodePreviews.MoveParametersIntoAb(ParamA, ParamB));
            sb.AppendLine("mul AB");
            return sb.ToString();
        }
    }
}