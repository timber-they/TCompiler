using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.IO
{
    public class Write : Command
    {
        private ByteVariable _port;
        private ByteVariable _toWrite;

        public Write(ByteVariable toWrite, ByteVariable port)
        {
            _toWrite = toWrite;
            _port = port;
        }
    }
}