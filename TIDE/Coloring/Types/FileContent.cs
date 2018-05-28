using System.IO;


namespace TIDE.Coloring.Types
{
    public class FileContent
    {
        public FileContent (string path)
        {
            Path = path;

            if (!File.Exists (Path))
                return;
            Content = File.ReadAllText (path);
        }

        public string Path    { get; }
        public string Content { get; }
    }
}