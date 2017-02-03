namespace TCompiler.Types.CompilingTypes.TemporaryOperation.TemporaryReturning
{
    /// <summary>
    /// Contains the stuff for a temporary returning
    /// </summary>
    public interface ITemporaryReturning
    {
        /// <summary>
        /// Evaluates the returning command of this temporary returning
        /// </summary>
        /// <returns></returns>
        ReturningCommand.ReturningCommand GetReturningCommand();
    }
}