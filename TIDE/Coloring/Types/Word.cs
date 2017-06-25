namespace TIDE.Coloring.Types
{
    /// <summary>
    ///     A word with it's position in the word array
    /// </summary>
    public class Word
    {
        /// <summary>
        ///     Initializes a new word
        /// </summary>
        /// <param name="value">The value (string) of the word</param>
        /// <param name="positionInWordArray">The position of the word in the word array</param>
        /// <param name="position">The real position in the text</param>
        public Word (string value, int positionInWordArray, int position)
        {
            Value = value;
            PositionInWordArray = positionInWordArray;
            Position = position;
        }

        /// <summary>
        ///     The position of the word in the word array
        /// </summary>
        public int PositionInWordArray { get; }

        /// <summary>
        ///     The value (string) of the word
        /// </summary>
        public string Value { get; }

        /// <summary>
        ///     The real position in the text
        /// </summary>
        public int Position { get; }
    }
}