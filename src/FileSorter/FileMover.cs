
    public class FileMover
    {
    public MoveResult MoveTo(SuggestedMove move) 
    {
        Directory.CreateDirectory(move.DestinationDirectory);
        string fileName = Path.GetFileName(move.File.FullPath);
        string finalPath = ResolveAvailablePath(Path.Combine(move.DestinationDirectory, fileName));
        File.Move(move.File.FullPath, finalPath);
        bool success = File.Exists(finalPath);
        return new MoveResult(move.File.FullPath, finalPath, success);
    }
    private string ResolveAvailablePath(string fullPath)
    {
        if (!File.Exists(fullPath)) return fullPath;
        string nameWithoutExtension = Path.GetFileNameWithoutExtension(fullPath);
        string extension = Path.GetExtension(fullPath);
        string directory = Path.GetDirectoryName(fullPath);
        int counter = 1;
        while (true)
        {
            string fullName = $"{nameWithoutExtension}({counter}){extension}";
            string newPath = Path.Combine(directory, fullName);
            if (!File.Exists(newPath)) return newPath;
            counter++;
        }
    }
    }

