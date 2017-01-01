using System.Collections.Generic;
using System.Text;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Method
{
    public class MethodCall : ReturningCommand
    {
        private Method Method { get; }
        private List<VariableCall> ParameterValues { get; }

        public MethodCall(Method method, List<VariableCall> parameterValues)
        {
            Method = method;
            ParameterValues = parameterValues;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (var i = 0; i < ParameterValues.Count; i++)
            {
                sb.AppendLine(ParameterValues[i].ToString());
                sb.AppendLine($"mov {Method.Parameters[i]}, A");
            }
            return $"{sb}\ncall {Method.Name}";
        }
    }
}