#region

using System.Text;

using TCompiler.AssembleHelp;
using TCompiler.Types.CompilerTypes;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.OneParameterOperation
{
    /// <summary>
    ///     Increases the parameter by 1 and writes the new value into the parameter<br />
    ///     Syntax:<br />
    ///     parameter++
    /// </summary>
    public class Increment : OneParameterOperation
    {
        /// <summary>
        ///     Initiates a new Increment
        /// </summary>
        /// <param name="parameter">The parameter to increase</param>
        /// <param name="cLine">The original T code line</param>
        public Increment(ByteVariableCall parameter, CodeLine cLine) : base(parameter, cLine)
        {
        }

        /// <summary>
        ///     Evaluates the stuff to execute in assembler to make an increment
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public override string ToString()
        {
            if (!((ByteVariableCall) Parameter).Variable.Address.IsInExtendedMemory)
                return $"{Ac.Increment} {((ByteVariableCall) Parameter).ByteVariable.Address}\n" +
                       ((ByteVariableCall) Parameter).ByteVariable.MoveThisIntoAccu();
            var sb = new StringBuilder();
            sb.AppendLine(((ByteVariableCall) Parameter).ByteVariable.Address.MoveThisIntoDataPointer());
            sb.AppendLine($"{Ac.MoveExtended} A, @dptr");
            sb.AppendLine($"{Ac.Increment} A");
            sb.AppendLine($"{Ac.MoveExtended} @dptr, A");
            return sb.ToString();
        }
    }
}