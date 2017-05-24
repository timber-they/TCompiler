using System.IO;

namespace TIDE.Coloring.Types
{
    public class FileContent
    {
        public string Path { get; }
        public string Content { get; }

        public FileContent(string path)
        {
            Path = path;

            if (!File.Exists(Path))
                return;
            Content = File.ReadAllText(path);
        }
    }
}