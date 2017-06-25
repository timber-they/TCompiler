namespace TIDE.Coloring.Types
{
    /// <summary>
    ///     Provides a range in a text
    /// </summary>
    public class Range
    {
        /// <summary>
        ///     Initializes a new range
        /// </summary>
        /// <param name="beginning">The beginning index of the range</param>
        /// <param name="ending">The ending index of the range</param>
        public Range (int beginning, int ending)
        {
            Beginning = beginning;
            Ending = ending;
        }

        /// <summary>
        ///     The beginning index of the range
        /// </summary>
        public int Beginning { get; }

        /// <summary>
        ///     The ending index of the range
        /// </summary>
        public int Ending { get; }
    }
}