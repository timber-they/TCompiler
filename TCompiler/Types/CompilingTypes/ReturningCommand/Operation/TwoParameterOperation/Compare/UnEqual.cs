#region

using System.Text;
using TCompiler.Compiling;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation.Compare
{
    /// <summary>
    /// Indicates wether parmA and paramB are unequal<br/>
    /// Syntax:<br/>
    /// paramA != paramB
    /// </summary>
    public class UnEqual : Compare
    {
        /// <summary>
        /// Initializes a new Unequal comparison
        /// </summary>
        /// <param name="paramA">The first parameter to compare</param>
        /// <param name="paramB">The second parameter to compare</param>
        public UnEqual(ByteVariableCall paramA, ByteVariableCall paramB) : base(paramA, paramB)
        {
        }

        /// <summary>
        /// Evaluates the stuff to execute in assembler to make an unequal comparison
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public override string ToString()
        {
            var notequal = ParseToAssembler.Label;
            var end = ParseToAssembler.Label;
            var a = (ByteVariableCall) ParamA;
            var b = (ByteVariableCall) ParamB;

            var sb = new StringBuilder();
            sb.AppendLine($"mov A, {a.ByteVariable}");
            sb.AppendLine($"cjne A, {b.ByteVariable}, {notequal.DestinationName}");
            sb.AppendLine("clr acc.0");
            sb.AppendLine($"jmp {end.DestinationName}");
            sb.AppendLine(notequal.LabelMark());
            sb.AppendLine("setb acc.0");
            sb.AppendLine(end.LabelMark());

            return sb.ToString();
        }
    }
}