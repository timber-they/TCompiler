#region

using System.Collections.Generic;
using System.Text;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Method
{
    public class MethodCall : ReturningCommand
    {
        public MethodCall(Method method, List<VariableCall> parameterValues) : base(true, true)
        {
            Method = method;
            ParameterValues = parameterValues;
        }

        private Method Method { get; }
        private List<VariableCall> ParameterValues { get; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (var i = 0; i < ParameterValues.Count; i++)
                sb.AppendLine(ParameterValues[i] + "\n" + (Method.Parameters[i] is ByteVariable
                                  ? $"mov {Method.Parameters[i]}, A"
                                  : $"mov C, acc.0\nmov {Method.Parameters[i].Address}, C"));
            return $"{sb}\ncall {Method.Label.DestinationName}";
        }
    }
}