namespace TCompiler.Types.CompilerTypes
{
    public class CodeLine
    {
        public CodeLine (string line, string fileName, int lineIndex)
        {
            Line      = line;
            FileName  = fileName;
            LineIndex = lineIndex;
        }

        public string Line      { get; }
        public string FileName  { get; }
        public int    LineIndex { get; }
    }
}