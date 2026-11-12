namespace FileSorter.Tests;

public class UnitTest1
{
    [Theory]
    [InlineData("C:\\Users\\Leroy\\Desktop\\Gulasch.JPEG", FileType.Image, ".jpeg")]
    [InlineData("C:\\Users\\Leroy\\Desktop\\Gulasch.avi", FileType.Video, ".avi")]
    [InlineData("C:\\Users\\Leroy\\Desktop\\Gulasch.MP3", FileType.Audio, ".mp3")]
    [InlineData("C:\\Users\\Leroy\\Desktop\\Gulasch.mobi", FileType.Book, ".mobi")]
    [InlineData("C:\\Users\\Leroy\\Desktop\\Gulasch.txt", FileType.Document, ".txt")]
    [InlineData("C:\\Users\\Leroy\\Desktop\\Gulasch.br", FileType.Other, ".br")]
    [InlineData("C:\\Users\\Leroy\\Desktop\\Gulasch", FileType.Other, "")]
    public void Analyze_FileExtensionsMatchAssignedType_ExtensionsMatchTypeFullPathPreservedAndExtensionCaseDoesNotMatter(string fullPath, FileType expectedType, string expectedExtension)
    {
        //Arrange
        Analyzer a1 = new Analyzer();
        //Act
        AnalyzedFile f = a1.Analyze(fullPath);
        //Assert
        Assert.Equal(fullPath, f.FullPath);
        Assert.Equal(expectedType, f.Type);
        Assert.Equal(expectedExtension, f.Extension);
    }

    [Fact]
    public void Scanner_ValidDirectory_ReturnsOnlyFilesInTopLevelDirectory()
    {
        string testDirectory1 = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        string testDirectory2 = Path.Combine(testDirectory1, "sub1");
        string f1 = Path.Combine(testDirectory1, "1.txt");
        string f2 = Path.Combine(testDirectory2, "2.txt");
        try {
            Directory.CreateDirectory(testDirectory1);
            Directory.CreateDirectory(testDirectory2);
            File.WriteAllText(f1, "");
            File.WriteAllText(f2, "");
            //Arrange
            Scanner s = new Scanner();

            //Act
            List<string> r = s.Scan(testDirectory1);
            //Assert
            Assert.Contains(f1, r);
            Assert.DoesNotContain(f2, r);
            Assert.Single(r);
        }
        finally
        {
            if (Directory.Exists(testDirectory1))
                Directory.Delete(testDirectory1, true);
        }

    }
    [Fact]
    public void Scanner_ValidPathIsEmpty_EmptyListReturned()
    {
        string testDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        try{
            Directory.CreateDirectory(testDirectory);
            //Arrange
            Scanner s = new Scanner();

        //Act
        List<string> r = s.Scan(testDirectory);
            //Assert
            Assert.Empty(r);
        }
        finally {
            if (Directory.Exists(testDirectory))
                Directory.Delete(testDirectory, true);
        }
    }
    [Fact]
    public void Scanner_InvalidPath_ExceptionThrown()
    {
        //Arrange
        Scanner s = new Scanner();
        string path = Path.Combine(
            Path.GetTempPath(),
            Guid.NewGuid().ToString()
        );
        //Act

        //Assert
        Assert.Throws<DirectoryNotFoundException>(() => s.Scan(path));
    }
}
