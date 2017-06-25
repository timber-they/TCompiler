using System.Drawing;


namespace MetaTextBoxLibrary
{
    public class ColoredCharacter
    {
        public ColoredCharacter (Color foreColor, Color backColor, char character)
        {
            ForeColor = foreColor;
            Character = character;
            BackColor = backColor;
        }

        public Color ForeColor { get; set; }
        public Color BackColor { get; set; }
        public char Character { get; }

        /// <inheritdoc />
        public override bool Equals (object obj) => obj is ColoredCharacter && Equals ((ColoredCharacter) obj);

        /// <inheritdoc />
        protected bool Equals (ColoredCharacter other) => ForeColor.Equals (other.ForeColor) &&
                                                          BackColor.Equals (other.BackColor) &&
                                                          Character == other.Character;

        public static bool operator == (ColoredCharacter left, ColoredCharacter right) => Equals (left, right);
        public static bool operator != (ColoredCharacter left, ColoredCharacter right) => !Equals (left, right);

        /// <inheritdoc />
        public override int GetHashCode ()
        {
            unchecked
            {
                // ReSharper disable once NonReadonlyMemberInGetHashCode
                return (ForeColor.GetHashCode () * 397) ^ Character.GetHashCode ();
            }
        }
    }
}