namespace TIDE.Coloring.Types
{
    /// <summary>
    /// A simple character with its position in the text
    /// </summary>
    public class Character
    {
        /// <summary>
        /// Initializes a new character
        /// </summary>
        /// <param name="value">The value of the character</param>
        /// <param name="position">The position in the text</param>
        public Character(char value, int position)
        {
            Position = position;
            Value = value;
        }

        /// <summary>
        /// The value of the character
        /// </summary>
        public char Value { get; }
        /// <summary>
        /// The positin in the text
        /// </summary>
        public int Position { get; }
    }
}