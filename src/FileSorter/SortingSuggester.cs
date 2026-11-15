public class SortingSuggester
{
    private static readonly Dictionary<FileType, string> fileNameMap = new()
    {
        [FileType.Image] = "Images",
        [FileType.Video] = "Videos",
        [FileType.Audio] = "Audio",
        [FileType.Book] = "Books",
        [FileType.Document] = "Documents",
        [FileType.Other] = "Other"
    };
    public string DestinationRoot { get; }
    public SortingSuggester(string destinationRoot)
    {
        DestinationRoot = destinationRoot;
    }
    public SuggestedMove Suggest(AnalyzedFile file)
    {
        return new SuggestedMove(file, Path.Combine(DestinationRoot, fileNameMap[file.Type]));
    }
}