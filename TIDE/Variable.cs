namespace TIDE
{
    public class Variable
    {
        public string Name { get; }
        public (int, int) VisibilityRangeLines { get; set; }
        public int Layer { get; }

        public Variable(string name, (int, int) visibilityRangeLines, int layer)
        {
            Name = name;
            VisibilityRangeLines = visibilityRangeLines;
            Layer = layer;
        }

        public Variable(string name, int visibilityRangeLinesStart, int layer)
        {
            Name = name;
            VisibilityRangeLines = (visibilityRangeLinesStart, -1);
            Layer = layer;
        }
    }
}