using System.Drawing;

namespace TIDE.Types
{
    public class StringColor
    {
        public Color Thecolor;
        public string Thestring;

        public StringColor(string thestring, Color thecolor)
        {
            Thestring = thestring;
            Thecolor = thecolor;
        }
    }
}