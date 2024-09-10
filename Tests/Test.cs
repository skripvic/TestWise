using Core;

namespace Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test_ProcessFile_RemovesShortWords()
    {
        const string inputText = "This is a test of the ProcessText method.";
        const string expectedOutput = "This   test   ProcessText method.";
        const int minWordLength = 4;
        const bool removePunctuation = false;
        var inputStream = new MemoryStream();
        var outputStream = new MemoryStream();

        using (var writer = new StreamWriter(inputStream, leaveOpen: true))
        {
            writer.Write(inputText);
            writer.Flush();
            inputStream.Position = 0;
        }

        using (var reader = new StreamReader(inputStream))
        using (var writer = new StreamWriter(outputStream))
        {
            var textProcessor = new TextProcessor();

            textProcessor.ProcessText(reader, writer, minWordLength, removePunctuation);
            writer.Flush();
            outputStream.Position = 0;
            
            using (var outputReader = new StreamReader(outputStream))
            {
                var result = outputReader.ReadToEnd();
                Assert.That(result, Is.EqualTo(expectedOutput));
            }
        }
    }

    [Test]
    public void Test_ProcessText_RemovePunctuation()
    {
        const string inputText = "Hello, world! This is a test.";
        const string expectedOutput = "Hello world This is a test";
        const int minWordLength = 0;
        const bool removePunctuation = true;
        var inputStream = new MemoryStream();
        var outputStream = new MemoryStream();

        using (var writer = new StreamWriter(inputStream, leaveOpen: true))
        {
            writer.Write(inputText);
            writer.Flush();
            inputStream.Position = 0;
        }

        using (var reader = new StreamReader(inputStream))
        using (var writer = new StreamWriter(outputStream))
        {
            var textProcessor = new TextProcessor();
            
            textProcessor.ProcessText(reader, writer, minWordLength, removePunctuation);
            writer.Flush();
            outputStream.Position = 0;
            
            using (var outputReader = new StreamReader(outputStream))
            {
                var result = outputReader.ReadToEnd();
                Assert.That(result, Is.EqualTo(expectedOutput));
            }
        }
    }
    
    [Test]
    public void Test_ProcessText_EmptyInput()
    {
        const string inputText = "";
        const string expectedOutput = "";
        const int minWordLength = 4;
        const bool removePunctuation = true;
        var inputStream = new MemoryStream();
        var outputStream = new MemoryStream();

        using (var writer = new StreamWriter(inputStream, leaveOpen: true))
        {
            writer.Write(inputText);
            writer.Flush();
            inputStream.Position = 0;
        }

        using (var reader = new StreamReader(inputStream))
        using (var writer = new StreamWriter(outputStream))
        {
            var textProcessor = new TextProcessor();

            textProcessor.ProcessText(reader, writer, minWordLength, removePunctuation);
            writer.Flush();
            outputStream.Position = 0;

            using (var outputReader = new StreamReader(outputStream))
            {
                var result = outputReader.ReadToEnd();
                Assert.That(result, Is.EqualTo(expectedOutput));
            }
        }
    }
    
    [Test]
    public void Test_ProcessText_WordIsBiggerThanBufferSize()
    {
        const string inputText = "tenlettersword men thatswonderfulthatswonderfulyay";
        const string expectedOutput = "tenlettersword  thatswonderfulthatswonderfulyay";
        const int minWordLength = 4;
        const bool removePunctuation = false;
        const int bufferSize = 7;
        const int maxSize = 10;
        var inputStream = new MemoryStream();
        var outputStream = new MemoryStream();

        using (var writer = new StreamWriter(inputStream, leaveOpen: true))
        {
            writer.Write(inputText);
            writer.Flush();
            inputStream.Position = 0;
        }

        using (var reader = new StreamReader(inputStream))
        using (var writer = new StreamWriter(outputStream))
        {
            var textProcessor = new TextProcessor(bufferSize, maxSize);

            textProcessor.ProcessText(reader, writer, minWordLength, removePunctuation);
            writer.Flush();
            outputStream.Position = 0;
            
            using (var outputReader = new StreamReader(outputStream))
            {
                var result = outputReader.ReadToEnd();
                Assert.That(result, Is.EqualTo(expectedOutput));
            }
        }
    }
    
    [Test]
    public void Test_ProcessText_WordIsBiggerThanMaxSize()
    {
        const string inputText = "tenlettersword men thatswonderfulthatswonderfulyay";
        const string expectedOutput = "tenlettersword  thatswonderfulthatswonderfulyay";
        const int minWordLength = 4;
        const bool removePunctuation = false;
        const int bufferSize = 11;
        const int maxSize = 10;
        var inputStream = new MemoryStream();
        var outputStream = new MemoryStream();

        using (var writer = new StreamWriter(inputStream, leaveOpen: true))
        {
            writer.Write(inputText);
            writer.Flush();
            inputStream.Position = 0;
        }

        using (var reader = new StreamReader(inputStream))
        using (var writer = new StreamWriter(outputStream))
        {
            var textProcessor = new TextProcessor(bufferSize, maxSize);

            textProcessor.ProcessText(reader, writer, minWordLength, removePunctuation);
            writer.Flush();
            outputStream.Position = 0;
            
            using (var outputReader = new StreamReader(outputStream))
            {
                var result = outputReader.ReadToEnd();
                Assert.That(result, Is.EqualTo(expectedOutput));
            }
        }
    }
    
    
}