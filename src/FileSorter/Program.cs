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
    foreach(string scanResult in scanResults)
    {
        AnalyzedFile outputFile = analyzer.Analyze(scanResult);
        SuggestedMove directorySuggestion = sortingSuggester.Suggest(outputFile);
        Console.WriteLine($"{directorySuggestion.File.FullPath} -> {directorySuggestion.DestinationDirectory}");
    }
}

Run();







