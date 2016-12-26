using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Types.CompilingTypes
{
    public class Sleep : Command
    {
        public Sleep(ByteVariableCall timeMz)
        {
            TimeMZ = timeMz;
        }

        public ByteVariableCall TimeMZ { get; }
    }
}