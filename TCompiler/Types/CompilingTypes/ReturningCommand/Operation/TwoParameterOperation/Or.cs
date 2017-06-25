#region

using System.Text;
using TCompiler.AssembleHelp;
using TCompiler.Types.CompilerTypes;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation
{
    /// <summary>
    ///     Represents the logical or operation<br />
    ///     Syntax:<br />
    ///     paramA | paramB
    /// </summary>
    public class Or : TwoParameterOperation
    {
        /// <summary>
        ///     Initializes a new or operation
        /// </summary>
        /// <param name="paramA">The first parameter for the or operation</param>
        /// <param name="paramB">The second parameter for the or operation</param>
        /// <param name="cLine">The original T code line</param>
        public Or(ReturningCommand paramA, ReturningCommand paramB, CodeLine cLine) : base(paramA, paramB, cLine)
        {
        }

        /// <summary>
        ///     Evaluates the stuff to execute in assembler to make a logical or operation
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(AssembleCodePreviews.MoveParametersIntoAb(ParamA, ParamB));
            sb.AppendLine($"{Ac.Or} A, 0F0h");
            return sb.ToString();
        }
    }
}