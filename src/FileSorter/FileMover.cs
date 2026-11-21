
    public class FileMover
    {
    public MoveResult MoveTo(SuggestedMove move) 
    {
        string sourcePath = move.File.FullPath;
        string fileName = Path.GetFileName(sourcePath);
        string finalPath = Path.Combine(move.DestinationDirectory, fileName);
        try
        {
            Directory.CreateDirectory(move.DestinationDirectory);
            finalPath = ResolveAvailablePath(finalPath);
            File.Move(sourcePath, finalPath);
            bool success = File.Exists(finalPath);
            return new MoveResult(sourcePath, finalPath, success);
        }
        catch (Exception ex)
        {
            return new MoveResult(sourcePath, finalPath, false, ex.Message);
        }
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

