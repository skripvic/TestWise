using System.Text;
using System.Text.RegularExpressions;

namespace Core;

public class TextProcessor
{
    private readonly int _bufferSize = 4096;
    private readonly int _maxLeftoverSize = 16384;

    public TextProcessor() { }

    public TextProcessor(int bufSize, int maxSize)
    {
        _bufferSize = bufSize;
        _maxLeftoverSize = maxSize;
    }

    public void ProcessFile(string inputFile, int minWordLength, bool removePunctuation, string outputFile)
    {
        using (var reader = new StreamReader(inputFile))
        using (var writer = new StreamWriter(outputFile))
        {
            ProcessText(reader, writer, minWordLength, removePunctuation);
        }
    }

    public void ProcessText(StreamReader reader, StreamWriter writer, int minWordLength, bool removePunctuation)
    {
        if (reader == null) throw new ArgumentNullException(nameof(reader));
        if (writer == null) throw new ArgumentNullException(nameof(writer));
        var isLeftoverCleared = false;
        var leftover = new StringBuilder();
        var buffer = new char[_bufferSize];

        while (!reader.EndOfStream)
        {
            var charsRead = reader.Read(buffer, 0, _bufferSize);

            ProcessBuffer(buffer, charsRead, leftover, reader, writer, ref isLeftoverCleared, minWordLength, removePunctuation);
        }

        WriteRemaining(leftover, writer, isLeftoverCleared, minWordLength, removePunctuation);
    }
    
    private void ProcessBuffer(char[] buffer, int charsRead, StringBuilder leftover, StreamReader reader, StreamWriter writer, ref bool isLeftoverCleared, int minWordLength, bool removePunctuation)
    {
        var lastNonLetterIndex = Array.FindLastIndex(buffer, charsRead - 1, charsRead, c => !char.IsLetter(c));

        if (lastNonLetterIndex != -1)
        {
            leftover.Append(buffer, 0, lastNonLetterIndex + 1);

            if (isLeftoverCleared)
            {
                isLeftoverCleared = false;
                HandleBigWordEnding(buffer, charsRead, leftover, reader, writer, minWordLength);
            }

            var bufferString = leftover.ToString();
            leftover.Clear();

            leftover.Append(buffer, lastNonLetterIndex + 1, charsRead - lastNonLetterIndex - 1);

            bufferString = ProcessPart(bufferString, removePunctuation, minWordLength);

            writer.Write(bufferString);
        }
        else
        {
            leftover.Append(buffer, 0, charsRead);

            if (leftover.Length <= _maxLeftoverSize) return;
            writer.Write(leftover.ToString());
            leftover.Clear();
            isLeftoverCleared = true;
        }
    }
    
    private void HandleBigWordEnding(char[] buffer, int charsRead, StringBuilder leftover, StreamReader reader, StreamWriter writer, int minWordLength)
    {
        var bigWordEnd = Array.FindIndex(buffer, 0, charsRead, c => !char.IsLetter(c));

        if (bigWordEnd <= minWordLength && bigWordEnd != -1)
        {
            leftover.Remove(0, bigWordEnd);

            var fakeShortWord = new StringBuilder();
            fakeShortWord.Append(buffer, 0, bigWordEnd);

            writer.Write(fakeShortWord);
        }
        else if (reader.EndOfStream && bigWordEnd == -1)
        {
            writer.Write(leftover);
        }
    }
    
    private void WriteRemaining(StringBuilder leftover, StreamWriter writer, bool isLeftoverCleared, int minWordLength, bool removePunctuation)
    {
        if (leftover.Length <= 0) return;
        var remainingString = leftover.ToString();

        if (!isLeftoverCleared)
        {
            remainingString = ProcessPart(remainingString, false, minWordLength);
        }

        writer.Write(remainingString);
    }

    private string ProcessPart(string text, bool removePunctuation, int minWordLength)
    {
        if (removePunctuation)
        {
            text = Regex.Replace(text, @"[\p{P}]", "");
        }

        if (minWordLength <= 1) return text;
        
        var regexPattern = $@"\b[\w-]{{1,{minWordLength - 1}}}\b";
        text = Regex.Replace(text.ToString(), regexPattern, "");

        return text;
    }
}
