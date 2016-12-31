using System.Drawing;

namespace TIDE.Types
{
    public class StringColor
    {
        public Color Thecolor { get; }
        public string Thestring { get; }

        public StringColor(string thestring, Color thecolor)
        {
            Thestring = thestring;
            Thecolor = thecolor;
        }
    }
}