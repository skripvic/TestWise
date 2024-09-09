namespace cadwiseTest;

public class FilePaths
{
    public string InputFilePath { get; private set; }
    public string OutputFilePath { get; private set; }
    
    public FilePaths(string inputFilePath, string outputFilePath)
    {
        InputFilePath = inputFilePath;
        OutputFilePath = outputFilePath;
    }
}