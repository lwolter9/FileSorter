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