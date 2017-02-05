#region

using System.Text;
using TCompiler.Types.CompilerTypes;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.OneParameterOperation
{
    /// <summary>
    ///     Decreases the parameter by 1 and writes the new value into the parameter<br />
    ///     Syntax:<br />
    ///     parameter--
    /// </summary>
    public class Decrement : OneParameterOperation
    {
        /// <summary>
        ///     Initializes a new decrement
        /// </summary>
        /// <param name="parameter">The variable to decrease</param>
        /// <param name="cLine">The original T code line</param>
        public Decrement(ReturningCommand parameter, CodeLine cLine) : base(parameter, cLine)
        {
        }

        /// <summary>
        ///     Evaluates the stuff to execute in assembler to make a decrement
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public override string ToString()
        {
            if (!((ByteVariableCall) Parameter).Variable.Address.IsInExtendedMemory)
                return $"dec {((ByteVariableCall) Parameter).ByteVariable.Address}\n" +
                       ((ByteVariableCall) Parameter).ByteVariable.MoveThisIntoAccu();
            var sb = new StringBuilder();
            sb.AppendLine(((ByteVariableCall) Parameter).ByteVariable.Address.MoveThisIntoDataPointer());
            sb.AppendLine("movx A, @dptr");
            sb.AppendLine("dec A");
            sb.AppendLine("movx @dptr, A");
            return sb.ToString();
        }
    }
}