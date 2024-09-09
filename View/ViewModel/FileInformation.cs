using System.ComponentModel;
using ViewModel;

namespace cadwiseTest;

public class FileInformation : INotifyPropertyChanged
{
    public string FilePath { get; set; }
    public int MinWordLength { get; set; }
    public bool RemovePunctuation { get; set; } 
    public string OutputFilePath { get; set; }

    private FileStatus _fileStatus;
    public FileStatus FileStatus
    {
        get => _fileStatus;
        set
        {
            if (_fileStatus == value) return;
            _fileStatus = value;
            OnPropertyChanged(nameof(FileStatus));
        }
    }

    public FileInformation(string filePath, int minWordLength, bool removePunctuation, FileStatus fileStatus, string outputFilePath)
    {
        FilePath = filePath;
        MinWordLength = minWordLength;
        RemovePunctuation = removePunctuation;
        FileStatus = fileStatus;
        OutputFilePath = outputFilePath;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
};