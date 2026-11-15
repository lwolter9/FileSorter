public class Scanner
{
    public List<string> Scan(string targetDirectory)
    {
        return Directory.GetFiles(targetDirectory).ToList();
    }
}