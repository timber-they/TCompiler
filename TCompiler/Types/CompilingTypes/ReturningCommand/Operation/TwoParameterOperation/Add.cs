#region

using System.Text;
using TCompiler.AssembleHelp;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation
{
    /// <summary>
    ///     Adds the two parameters<br />
    ///     Syntax:<br />
    ///     paramA + paramB
    /// </summary>
    public class Add : TwoParameterOperation
    {
        /// <summary>
        ///     Initiates a new Add operation
        /// </summary>
        /// <param name="paramA">The first parameter to add</param>
        /// <param name="paramB">The second parameter to add</param>
        public Add(ReturningCommand paramA, ReturningCommand paramB) : base(paramA, paramB)
        {
        }

        /// <summary>
        ///     Evaluates the stuff to execute in assembler to make an add operation
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(AssembleCodePreviews.MoveParametersIntoAb(ParamA, ParamB));
            sb.AppendLine("add A, 0F0h");
            return sb.ToString();
        }
    }
}