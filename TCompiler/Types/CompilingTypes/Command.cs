namespace TCompiler.Types.CompilingTypes
{
    /// <summary>
    /// The base for every command you can type in T
    /// </summary>
    public abstract class Command
    {
        protected Command(bool deactivateEa, bool activateEa)
        {
            DeactivateEa = deactivateEa;
            ActivateEa = activateEa;
        }

        public bool DeactivateEa { get; }
        public bool ActivateEa { get; }
    }
}