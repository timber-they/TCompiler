using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.IO
{
    public class Read : ReturningCommand
    {
        private ByteVariable _port;

        public Read(ByteVariable port)
        {
            _port = port;
        }
    }
}