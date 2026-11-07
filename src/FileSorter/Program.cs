using System.Runtime.CompilerServices;

Console.WriteLine("Hello, World!");

public enum FileType
{
    Image, Video, Audio, Book, Document, Other
}
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

public class SuggestedMove
{
    public AnalyzedFile File { get; }
    public string DestinationDirectory { get; }

    public SuggestedMove(AnalyzedFile file, string destinationDirectory)
    {
        File = file;
        DestinationDirectory = destinationDirectory;
    }
}