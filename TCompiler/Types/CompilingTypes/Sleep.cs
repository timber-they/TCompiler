using TCompiler.Types.CompilerTypes;


namespace TCompiler.Types.CompilingTypes
{
    /// <summary>
    ///     A sleep command that uses loops to wait
    /// </summary>
    public class Sleep : Command
    {
        /// <summary>
        ///     Initiates a new sleep command
        /// </summary>
        /// <returns>Nothing</returns>
        /// <param name="timeMs">The time to sleep in milliseconds</param>
        /// <param name="tCode">The original T code line</param>
        public Sleep (int timeMs, CodeLine tCode) : base (true, true, tCode) => TimeMs = timeMs;

        /// <summary>
        ///     The time to sleep
        /// </summary>
        /// <value>The time as an integer in milliseconds</value>
        public int TimeMs { get; }
    }
}