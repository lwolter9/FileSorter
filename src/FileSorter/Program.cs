using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;


void Run() 
{
    Console.WriteLine("Enter the directory to be sorted.");
    string sortingDirectory = Console.ReadLine();
    Scanner scanner = new Scanner();
    List<string> scanResults = scanner.Scan(sortingDirectory);
    Analyzer analyzer = new Analyzer();
    Console.WriteLine("Enter the desired destination directory.");
    string destinationDirectory = Console.ReadLine();
    SortingSuggester sortingSuggester = new SortingSuggester(destinationDirectory);
    List<SuggestedMove> suggestions = new List<SuggestedMove>();
    foreach(string scanResult in scanResults)
    {
        AnalyzedFile outputFile = analyzer.Analyze(scanResult);
        suggestions.Add(sortingSuggester.Suggest(outputFile));
    }
    foreach(SuggestedMove suggestion in suggestions)
    {
        Console.WriteLine($"{suggestion.File.FullPath} -> {suggestion.DestinationDirectory}");
    }
    Console.WriteLine("Proceed with suggested moves? (Y/N): ");
    string response = Console.ReadLine();
    List<MoveResult> results = new List<MoveResult>();

    if (string.Equals(response, "y", StringComparison.OrdinalIgnoreCase))
    {
        FileMover mover = new FileMover();
        foreach (SuggestedMove suggestion in suggestions)
        {
            results.Add(mover.MoveTo(suggestion));
        }
        List<MoveResult> successes = results.Where(move => move.Success == true).ToList();
        List<MoveResult> renames = results.Where(move => move.WasRenamed == true).ToList();
        List<MoveResult> fails = results.Where(move => move.Success == false).ToList();
        Console.WriteLine("Sorting complete.\n");
        Console.WriteLine($"Moved successfully: {successes.Count}");
        Console.WriteLine($"Renamed due to collisions: {renames.Count}");
        Console.WriteLine($"Failed: {fails.Count}\n");
        Console.WriteLine("Renamed Files:");
        foreach(MoveResult rename in renames)
        {
            Console.WriteLine($"{Path.GetFileName(rename.SourcePath)} -> {Path.GetFileName(rename.FinalPath)}");
        }
        Console.WriteLine("\nFailed moves:");
        foreach (MoveResult fail in fails)
        {
            Console.WriteLine($"{Path.GetFileName(fail.SourcePath)}");
            Console.WriteLine($"Reason: {fail.ErrorMessage}");
        }
    }
    else
    {
        Console.WriteLine("Sorting cancelled. No files were moved.");
    }
}

Run();







