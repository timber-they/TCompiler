using System.Windows.Media;

namespace TIDE.Types
{
    public class Range
    {
        public Range(Brush color, int start, int end)
        {
            Color = color;
            End = end;
            Start = start;
        }

        public int Start { get; }
        public int End { get; }
        public Brush Color { get; }
    }
}