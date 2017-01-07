#region

using TCompiler.Compiling;
using TCompiler.Types.CheckTypes.TCompileException;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

namespace TCompiler.Types.CompilingTypes
{
    /// <summary>
    /// A sleep command that uses loops to wait
    /// </summary>
    public class Sleep : Command
    {
        /// <summary>
        /// Initiates a new sleep command
        /// </summary>
        /// <returns>Nothing</returns>
        public Sleep(ByteVariableCall timeMs)
        {
            if (!timeMs.Variable.IsConstant)
                throw new ParameterException(ParseToObjects.Line, "Sleep must have a constant parameter!");
            TimeMs = timeMs;
        }

        /// <summary>
        /// The time to sleep
        /// </summary>
        /// <value>The time as an integer in milliseconds</value>
        public ByteVariableCall TimeMs { get; }
    }
}