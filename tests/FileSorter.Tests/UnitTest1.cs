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
    [Theory]
    [InlineData("C:\\Users\\Leroy\\Desktop\\Gulasch.JPEG", FileType.Image, ".jpeg", "Images")]
    [InlineData("C:\\Users\\Leroy\\Desktop\\Gulasch.avi", FileType.Video, ".avi", "Videos")]
    [InlineData("C:\\Users\\Leroy\\Desktop\\Gulasch.MP3", FileType.Audio, ".mp3", "Audio")]
    [InlineData("C:\\Users\\Leroy\\Desktop\\Gulasch.mobi", FileType.Book, ".mobi", "Books")]
    [InlineData("C:\\Users\\Leroy\\Desktop\\Gulasch.txt", FileType.Document, ".txt", "Documents")]
    [InlineData("C:\\Users\\Leroy\\Desktop\\Gulasch", FileType.Other, "", "Other")]
    public void Suggest_FileTypeCorrectlyMaps_DestinationDirectoryMatchesInputAndAnalyzedFileIsMaintained(string fullPath, FileType expectedType, string expectedExtension, string expectedFolder)
    {
        //Arrange
        SortingSuggester s = new SortingSuggester("C:\\Desktop\\Sorted Files");
        AnalyzedFile f = new AnalyzedFile(fullPath, expectedExtension, expectedType);
        //Act
        SuggestedMove m = s.Suggest(f);
        //Assert
        Assert.Equal(Path.Combine(s.DestinationRoot, expectedFolder), m.DestinationDirectory);
        Assert.Same(f, m.File);
    }
    [Fact]
    public void MoveTo_ValidMove_FileIsMovedAndResultIsSuccessful()
    {
        string testDirectory1 = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        string testDirectory2 = Path.Combine(testDirectory1, "sub1");
        string f1 = Path.Combine(testDirectory1, "1.txt");
        try
        {
            Directory.CreateDirectory(testDirectory1);
            Directory.CreateDirectory(testDirectory2);
            File.WriteAllText(f1, "");
            //Arrange
            Analyzer a = new Analyzer();
            AnalyzedFile f = a.Analyze(f1);
            SortingSuggester s = new SortingSuggester(testDirectory2);
            SuggestedMove sm = s.Suggest(f);
            FileMover m = new FileMover();
            string fp = Path.Combine(testDirectory2, "Documents", "1.txt");
            //Act
            MoveResult r = m.MoveTo(sm);
            //Assert
            Assert.True(Directory.Exists(Path.Combine(testDirectory2, "Documents")));
            Assert.True(File.Exists(fp));
            Assert.False(File.Exists(f1));
            Assert.Equal(fp, r.FinalPath);
            Assert.Equal(f1, r.SourcePath);
            Assert.True(r.Success);
            Assert.False(r.WasRenamed);
        }
        finally
        {
            if (Directory.Exists(testDirectory1))
                Directory.Delete(testDirectory1, true);
        }

    }
    [Fact]
    public void MoveTo_FileWithSameNameExistsAtDestination_FileIsMovedRenamedAndResultIsSuccessful()
    {
        string testDirectory1 = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        string testDirectory2 = Path.Combine(testDirectory1, "sub1");
        string testDirectory3 = Path.Combine(testDirectory2, "Documents");
        string f1 = Path.Combine(testDirectory1, "1.txt");
        string f2 = Path.Combine(testDirectory3, "1.txt");
        try
        {
            Directory.CreateDirectory(testDirectory1);
            Directory.CreateDirectory(testDirectory3);
            File.WriteAllText(f1, "Testing");
            File.WriteAllText(f2, "Successful");
            //Arrange
            Analyzer a = new Analyzer();
            AnalyzedFile f = a.Analyze(f1);
            SortingSuggester s = new SortingSuggester(testDirectory2);
            SuggestedMove sm = s.Suggest(f);
            FileMover m = new FileMover();
            string fp = Path.Combine(testDirectory3, "1(1).txt");
            //Act
            MoveResult r = m.MoveTo(sm);
            //Assert
            Assert.True(Directory.Exists(Path.Combine(testDirectory2, "Documents")));
            Assert.True(File.Exists(fp));
            Assert.False(File.Exists(f1));
            Assert.Equal(fp, r.FinalPath);
            Assert.Equal(f1, r.SourcePath);
            Assert.True(r.Success);
            Assert.True(r.WasRenamed);
            Assert.Equal("Successful", File.ReadAllText(f2));
            Assert.Equal("Testing", File.ReadAllText(fp));
        }
        finally
        {
            if (Directory.Exists(testDirectory1))
                Directory.Delete(testDirectory1, true);
        }

    }
    [Fact]
    public void MoveTo_FileWithSameNameExistsAtDestinationMultipleTimes_FileIsMovedRenamedAppropriatelyAndResultIsSuccessful()
    {
        string testDirectory1 = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        string testDirectory2 = Path.Combine(testDirectory1, "sub1");
        string testDirectory3 = Path.Combine(testDirectory2, "Documents");
        string f1 = Path.Combine(testDirectory1, "1.txt");
        string f2 = Path.Combine(testDirectory3, "1.txt");
        string f3 = Path.Combine(testDirectory3, "1(1).txt");
        string f4 = Path.Combine(testDirectory3, "1(2).txt");
        try
        {
            Directory.CreateDirectory(testDirectory1);
            Directory.CreateDirectory(testDirectory3);
            File.WriteAllText(f1, "Testing");
            File.WriteAllText(f2, "Successful");
            File.WriteAllText(f3, "Successful");
            File.WriteAllText(f4, "Successful");

            //Arrange
            Analyzer a = new Analyzer();
            AnalyzedFile f = a.Analyze(f1);
            SortingSuggester s = new SortingSuggester(testDirectory2);
            SuggestedMove sm = s.Suggest(f);
            FileMover m = new FileMover();
            string fp = Path.Combine(testDirectory3, "1(3).txt");
            //Act
            MoveResult r = m.MoveTo(sm);
            //Assert
            Assert.True(Directory.Exists(Path.Combine(testDirectory2, "Documents")));
            Assert.True(File.Exists(fp));
            Assert.False(File.Exists(f1));
            Assert.Equal(fp, r.FinalPath);
            Assert.Equal(f1, r.SourcePath);
            Assert.True(r.Success);
            Assert.True(r.WasRenamed);
            Assert.Equal("Successful", File.ReadAllText(f2));
            Assert.Equal("Testing", File.ReadAllText(fp));
            Assert.Equal("Successful", File.ReadAllText(f3));
            Assert.Equal("Successful", File.ReadAllText(f4));
        }
        finally
        {
            if (Directory.Exists(testDirectory1))
                Directory.Delete(testDirectory1, true);
        }


    }
    [Fact]
    public void MoveTo_InvalidMove_FileRemainsAndResultContainsCorrectState()
    {
        string testDirectory1 = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        string testDirectory2 = Path.Combine(testDirectory1, "sub1");
        string f1 = Path.Combine(testDirectory1, "1.txt");
        try
        {
            Directory.CreateDirectory(testDirectory1);
            File.WriteAllText(testDirectory2, "");
            File.WriteAllText(f1, "Testing");
            //Arrange
            Analyzer a = new Analyzer();
            AnalyzedFile f = a.Analyze(f1);
            SortingSuggester s = new SortingSuggester(testDirectory2);
            SuggestedMove sm = s.Suggest(f);
            FileMover m = new FileMover();
            string fp = Path.Combine(testDirectory2, "Documents", "1.txt");
            //Act
            MoveResult r = m.MoveTo(sm);
            //Assert
            Assert.True(File.Exists(f1));
            Assert.Equal(fp, r.FinalPath);
            Assert.Equal(f1, r.SourcePath);
            Assert.False(r.Success);
            Assert.False(string.IsNullOrWhiteSpace(r.ErrorMessage));
        }
        finally
        {
            if (Directory.Exists(testDirectory1))
                Directory.Delete(testDirectory1, true);
        }

    }
}
