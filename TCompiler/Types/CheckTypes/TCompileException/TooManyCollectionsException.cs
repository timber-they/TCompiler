namespace TCompiler.Types.CheckTypes.TCompileException
{
    /// <summary>
    /// Gets thrown when the collections don't find some place to live in the internal RAM.
    /// </summary>
    /// <remarks>Is only needed because I didn't put them into the xMem yet.</remarks>
    public class TooManyCollectionsException : TooManyException
    {
        /// <summary>
        /// Initializes a new TooManyCollections exception
        /// </summary>
        /// <param name="lineIndex">The index of the line in which the exception got thrown</param>
        /// <param name="message">The message to view to the user</param>
        public TooManyCollectionsException(int lineIndex, string message="Actually the collection can't be in the extended memory yet!") : base(lineIndex, message)
        {
        }
    }
}