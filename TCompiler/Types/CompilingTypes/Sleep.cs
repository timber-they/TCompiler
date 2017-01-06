#region

using TCompiler.Compiling;
using TCompiler.Types.CheckTypes.TCompileException;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

namespace TCompiler.Types.CompilingTypes
{
    public class Sleep : Command
    {
        public Sleep(ByteVariableCall timeMs)
        {
            if (!timeMs.Variable.IsConstant)
                throw new ParameterException(ParseToObjects.Line, "Sleep must have a constant parameter!");
            TimeMs = timeMs;
        }

        public ByteVariableCall TimeMs { get; }
    }
}