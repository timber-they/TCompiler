using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace MetaTextBox
{
    public class ColoredString
    {
        public ColoredString (List<ColoredCharacter> coloredCharacters) => ColoredCharacters = coloredCharacters;

        public ColoredString (Color foreColor, Color backColor, string @string) => ColoredCharacters =
                                                                                       @string?.ToCharArray ().
                                                                                                Select (
                                                                                                    c =>
                                                                                                        new
                                                                                                            ColoredCharacter (
                                                                                                                foreColor,
                                                                                                                backColor,
                                                                                                                c)).
                                                                                                ToList ();

        public ColoredString (ColoredString oldColoredString) =>
            ColoredCharacters = oldColoredString.ColoredCharacters == null
                                    ? null
                                    : new List<ColoredCharacter> (oldColoredString.ColoredCharacters);

        public ColoredString (IEnumerable<ColoredCharacter> oldColoredString) : this (oldColoredString.ToList ())
        {
        }

        public List<ColoredCharacter> ColoredCharacters { get; }

        public ColoredString Replace (ColoredCharacter toReplace, ColoredCharacter replacement) => new ColoredString (
            ColoredCharacters.Select (character => character == toReplace ? replacement : character));

        public ColoredString Remove (ColoredCharacter toRemove) => new ColoredString (
            ColoredCharacters.Where (character => character != toRemove));

        public ColoredString Remove (char toRemove) => new ColoredString (
            ColoredCharacters.Where (character => character.Character != toRemove));

        public ColoredString Remove (int index, int count)
        {
            if (index < 0 || index > ColoredCharacters.Count() || count < 0 || index + count > ColoredCharacters.Count())
                throw new IndexOutOfRangeException ();
            return new ColoredString (
                ColoredCharacters.Where ((_, i) => i < index || i >= index + count));
        }

        public ColoredString Insert (int index, ColoredString coloredString)
        {
            var l = new List<ColoredCharacter> (ColoredCharacters);
            l.InsertRange (index, coloredString.ColoredCharacters);
            return new ColoredString (l);
        }

        public ColoredString Insert (int index, ColoredCharacter coloredCharacter)
        {
            var l = new List<ColoredCharacter> (ColoredCharacters);
            l.Insert (index, coloredCharacter);
            return new ColoredString (l);
        }

        public List<ColoredString> Split (ColoredCharacter splitter)
        {
            var final = new List<ColoredString> ();
            var current = new List<ColoredCharacter> ();
            foreach (var coloredCharacter in ColoredCharacters)
                if (coloredCharacter == splitter)
                {
                    final.Add (new ColoredString (current));
                    current = new List<ColoredCharacter> ();
                }
                else
                    current.Add (coloredCharacter);
            final.Add (new ColoredString (current));
            return final;
        }

        public List<ColoredString> Split (char splitter, bool keepSplitter = false)
        {
            var final = new List<ColoredString> ();
            var current = new List<ColoredCharacter> ();
            foreach (var coloredCharacter in ColoredCharacters)
                if (coloredCharacter.Character == splitter)
                {
                    if (keepSplitter)
                        current.Add(coloredCharacter);
                    final.Add (new ColoredString (current));
                    current = new List<ColoredCharacter> ();
                }
                else
                    current.Add (coloredCharacter);
            final.Add (new ColoredString (current));
            return final;
        }

        public bool Contains (ColoredCharacter character) => ColoredCharacters.Contains (character);

        public bool Contains (char character) => ColoredCharacters.Any (
            coloredCharacter => coloredCharacter.Character == character);

        public ColoredCharacter Get (int index) => ColoredCharacters.ToList() [index];

        public void Set (int index, ColoredCharacter coloredCharacter) => ColoredCharacters.ToList () [index] = coloredCharacter;

        public void SetForeColor (int index, Color color) => ColoredCharacters.ToList () [index].ForeColor = color;

        public void SetForeColorRange (int index, int count, Color color)
        {
            for (var i = 0; i < count; i++)
                ColoredCharacters.ToList () [index + i].ForeColor = color;
        }

        public void SetBackColor (int index, Color color) => ColoredCharacters.ToList () [index].BackColor = color;

        public void SetBackColorRange (int index, int count, Color color)
        {
            for (var i = 0; i < count; i++)
                ColoredCharacters.ToList () [index + i].BackColor = color;
        }

        public Color? GetFirstOrDefaultForeColor () => ColoredCharacters.FirstOrDefault ()?.ForeColor;

        public Color? GetFirstOrDefaultBackColor () => ColoredCharacters.FirstOrDefault ()?.BackColor;

        public int Count () => ColoredCharacters.Count();

        public ColoredString Substring (int index)
        {
            if (index < 0 || index > ColoredCharacters.Count())
                throw new IndexOutOfRangeException ();
            return new ColoredString (ColoredCharacters.Skip (index));
        }

        public ColoredString Substring (int index, int count)
        {
            if (index < 0 || count < 0 || index + count > ColoredCharacters.Count())
                throw new IndexOutOfRangeException ();
            return new ColoredString (
                ColoredCharacters.Skip (index).Take (count));
        }

        public ColoredCharacter LastOrDefault () => ColoredCharacters?.LastOrDefault ();

        /// <inheritdoc />
        public override bool Equals (object obj) => obj is ColoredString && Equals ((ColoredString) obj);

        /// <inheritdoc />
        protected bool Equals (ColoredString other) => other != null &&
                                                       (other.ColoredCharacters == ColoredCharacters ||
                                                        other.ColoredCharacters != null &&
                                                        ColoredCharacters != null &&
                                                        other.Count () == Count () &&
                                                        other.ColoredCharacters.
                                                              Select ((character, i) => new {character, i}).
                                                              All (arg => arg.character == ColoredCharacters.ToList () [arg.i]));

        public static bool operator == (ColoredString left, ColoredString right) => Equals (left, right);
        public static bool operator != (ColoredString left, ColoredString right) => !Equals (left, right);

        public static ColoredString operator + (ColoredString left, ColoredString right) => new ColoredString (
            left.ColoredCharacters.Concat (right.ColoredCharacters));

        public static ColoredString operator + (ColoredString left, ColoredCharacter right) => new ColoredString (
            left.ColoredCharacters.Concat (new List<ColoredCharacter> {right}));

        /// <inheritdoc />
        public override int GetHashCode () => (ColoredCharacters != null ? ColoredCharacters.GetHashCode () : 0);

        /// <inheritdoc />
        public override string ToString () => ColoredCharacters == null
                                                  ? ""
                                                  : string.Join (
                                                      "", ColoredCharacters.Select (character => character.Character));
    }
}