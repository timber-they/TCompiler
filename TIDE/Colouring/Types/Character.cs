namespace TIDE.Colouring.Types
{
    public class Character
    {
        public Character(char value, int position)
        {
            this.Position = position;
            this.Value = value;
        }

        public char Value { get; }
        public int Position { get; }
    }
}