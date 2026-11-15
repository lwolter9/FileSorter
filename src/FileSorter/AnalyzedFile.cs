public class AnalyzedFile
{
    public string FullPath { get; }
    public string Extension { get; }
    public FileType Type { get; }

    public AnalyzedFile(string fullPath, string extension, FileType type)
    {
        FullPath = fullPath;
        Extension = extension;
        Type = type;
    }
}