#region

using System.Text;
using TCompiler.Compiling;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation.Compare
{
    public class Bigger : Compare
    {
        public Bigger(ByteVariableCall paramA, ByteVariableCall paramB) : base(paramA, paramB)
        {
        }

        public override string ToString()
        {
            var notequal = ParseToAssembler.Label;
            var end = ParseToAssembler.Label;
            var bigger = ParseToAssembler.Label;
            var a = (ByteVariableCall) ParamA;
            var b = (ByteVariableCall) ParamB;

            var sb = new StringBuilder();
            sb.AppendLine($"mov A, {a.ByteVariable}");
            sb.AppendLine($"cjne A, {b.ByteVariable}, {notequal.DestinationName}");
            sb.AppendLine("clr acc.0");
            sb.AppendLine($"jmp {end.DestinationName}");
            sb.AppendLine(notequal.LabelMark());
            sb.AppendLine($"jnc {bigger.DestinationName}");
            sb.AppendLine("clr acc.0");
            sb.AppendLine($"jmp {end.DestinationName}");
            sb.AppendLine(bigger.LabelMark());
            sb.AppendLine("setb acc.0");
            sb.AppendLine(end.LabelMark());

            return sb.ToString();
        }
    }
}