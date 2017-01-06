#region

using System.Drawing;

#endregion

namespace TIDE.Colouring.Types
{
    public class StringColor
    {
        public StringColor(string thestring, Color thecolor)
        {
            Thestring = thestring;
            Thecolor = thecolor;
        }

        public Color Thecolor { get; }
        public string Thestring { get; }
    }
}