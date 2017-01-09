namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation
{
    public abstract class Operation : ReturningCommand
    {
        protected Operation(bool deactivateEa, bool activateEa) : base(deactivateEa, activateEa)
        {
        }
    }
}