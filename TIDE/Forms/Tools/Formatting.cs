using System.Linq;
using System.Text;

namespace TIDE.Forms.Tools
{
    public static class Formatting
    {
        public static string GetFormattedText(string text)
        {
            var lines = text.Split('\n').Select(s => s.Trim('\r').Trim());
            var layer = 0;
            var res = new StringBuilder();
            foreach (var line in lines)
            {
                if (PublicStuff.EndCommands.Any(s => line.StartsWith(s)))
                    layer--;
                res.AppendLine($"{string.Join("", Enumerable.Repeat(' ', layer * 4))}{line}");
                if (PublicStuff.BeginningCommands.Any(s => line.StartsWith(s)))
                    layer++;
            }
            return res.ToString();
        }
    }
}