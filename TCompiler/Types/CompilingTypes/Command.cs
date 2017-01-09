namespace TCompiler.Types.CompilingTypes
{
    /// <summary>
    /// The base for every command you can type in T
    /// </summary>
    public abstract class Command
    {
        protected Command(bool isSingleLine)
        {
            IsSingleLine = isSingleLine;
        }

        public bool IsSingleLine { get; }
    }
}