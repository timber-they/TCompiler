namespace TCompiler.Types.CompilingTypes
{
    /// <summary>
    ///     The base for every command you can type in T
    /// </summary>
    public abstract class Command
    {
        /// <summary>
        ///     Initiates a new Command
        /// </summary>
        /// <param name="deactivateEa">A boolean that indicates wether the enableAll flag must get deactivated before the command</param>
        /// <param name="activateEa">A boolean that indicates wether the enableAll flag must get activated after the command</param>
        protected Command(bool deactivateEa, bool activateEa)
        {
            DeactivateEa = deactivateEa;
            ActivateEa = activateEa;
        }

        /// <summary>
        ///     A boolean that indicates wether the enableAll flag must get deactivated before the command
        /// </summary>
        public bool DeactivateEa { get; }

        /// <summary>
        ///     A boolean that indicates wether the enableAll flag must get activated after the command
        /// </summary>
        public bool ActivateEa { get; }
    }
}