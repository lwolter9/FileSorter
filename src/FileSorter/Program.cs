using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;

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
public class Analyzer
{
    private static readonly HashSet<string> image = [".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp"];
    private static readonly HashSet<string> video = [".mp4", ".mkv", ".avi", ".mov", ".wmv"];
    private static readonly HashSet<string> audio = [".mp3", ".wav", ".flac", ".aac", ".m4a"];
    private static readonly HashSet<string> book = [".epub", ".mobi", ".azw", ".azw3"];
    private static readonly HashSet<string> document = [".pdf", ".doc", ".docx", ".txt", ".rtf", ".xls", ".xlsx", ".ppt", ".pptx"];
    public AnalyzedFile Analyze(string fullPath)
    {
        string extension = Path.GetExtension(fullPath).ToLower();
        FileType type = IdentifyType(extension);
        return new AnalyzedFile(fullPath, extension, type);
    }
    private FileType IdentifyType(string extension)
    {

        if (image.Contains(extension)) return FileType.Image;
        if (video.Contains(extension)) return FileType.Video;
        if (audio.Contains(extension)) return FileType.Audio;
        if (book.Contains(extension)) return FileType.Book;
        if (document.Contains(extension)) return FileType.Document;
        return FileType.Other;
    }
}
public class Scanner
{
    public List<string> Scan(string targetDirectory)
    {
        if (!Directory.Exists(targetDirectory)) throw new DirectoryNotFoundException();
        List<string> filePaths = Directory.GetFiles(targetDirectory).ToList();
        return filePaths;
    }
}