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
}
