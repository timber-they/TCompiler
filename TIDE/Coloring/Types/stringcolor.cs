#region

using System.Drawing;

#endregion

namespace TIDE.Coloring.Types
{
    /// <summary>
    ///     A string and the fitting color
    /// </summary>
    public class StringColor
    {
        /// <summary>
        ///     Initializes a new stringColor
        /// </summary>
        /// <param name="thestring">The string for the color</param>
        /// <param name="thecolor">The color of the string</param>
        public StringColor(string thestring, Color thecolor)
        {
            Thestring = thestring;
            Thecolor = thecolor;
        }

        /// <summary>
        ///     The color of the string
        /// </summary>
        public Color Thecolor { get; }

        /// <summary>
        ///     The string for the color
        /// </summary>
        public string Thestring { get; }
    }
}