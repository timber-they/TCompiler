using TCompiler.Types.CompilerTypes;
using TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation;


namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    public class CollectionCall : VariableCall
    {
        public CollectionCall (Collection variable, CodeLine cLine) : base (variable, cLine) {}

        public override string ToString ()
            =>
                new VariableOfCollection ((Collection) Variable,
                                          new ByteVariableCall (new Int (null, "1", true), TCode),
                                          TCode).ToString
                    ();
    }
}