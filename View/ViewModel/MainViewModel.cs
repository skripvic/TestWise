using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using cadwiseTest;
using Core;

namespace ViewModel;

public class MainViewModel : INotifyPropertyChanged
{
    private bool _removePunctuation;
    private bool _removeShortWords;
    private int _minWordLength = 1;
    private bool _isButtonEnabled = true;
    private TextProcessor _textProcessor;

    public ObservableCollection<FilePaths> SelectedFilePaths { get; private set; }
    public ObservableCollection<FileInformation> SelectedFiles { get; private set; }
    public ICommand SelectFilesCommand { get; }
    public ICommand StartProcessingCommand { get; }
    
    
    public MainViewModel()
    {
        _textProcessor = new TextProcessor();
        SelectedFilePaths = new ObservableCollection<FilePaths>();
        SelectedFiles = new ObservableCollection<FileInformation>();
        SelectFilesCommand = new RelayCommand(SelectFiles);
        StartProcessingCommand = new RelayCommand(StartProcessing);
    }
    

    public bool IsButtonEnabled
    {
        get => _isButtonEnabled;
        private set
        {
            if (_isButtonEnabled != value)
            {
                _isButtonEnabled = value;
                OnPropertyChanged();
            }
        }
    }

    public bool RemovePunctuation
    {
        get => _removePunctuation;
        set
        {
            _removePunctuation = value;
            OnPropertyChanged();
        }
    }

    public bool RemoveShortWords
    {
        get => _removeShortWords;
        set
        {
            _removeShortWords = value;
            OnPropertyChanged();
        }
    }

    public int MinWordLength
    {
        get => _minWordLength;
        set
        {
            _minWordLength = value;
            OnPropertyChanged();
        }
    }

    private void SelectFiles(object selectedFilePaths)
    {
        if (selectedFilePaths is not FilePaths filePaths) return;
        SelectedFilePaths.Add(filePaths);
        IsButtonEnabled = SelectedFilePaths.Count < 10;
    }


    private void StartProcessing(object obj)
    {
        if (!_removeShortWords)
        {
            _minWordLength = 1;
        }
        foreach (var selectedFilePath in SelectedFilePaths)
        {
            if (SelectedFiles.Count >= 30)
            {
                SelectedFiles.RemoveAt(0);
            }

            SelectedFiles.Add(new FileInformation(selectedFilePath.InputFilePath, _minWordLength,
                _removePunctuation, FileStatus.NotStarted, selectedFilePath.OutputFilePath!));
        }
        ProcessFilesAsync();
        ResetParameters();
    }

    private async void ProcessFilesAsync()
    {
        foreach (var file in SelectedFiles)
        {
            if (file.FileStatus != FileStatus.NotStarted) continue;
            file.FileStatus = FileStatus.InProgress;
            await Task.Run(() =>
            {
                try
                {
                    _textProcessor.ProcessFile(file.FilePath, file.MinWordLength, file.RemovePunctuation, file.OutputFilePath);
                    file.FileStatus = FileStatus.Done;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при обработке файла {file.FilePath}: {ex.Message}");
                    file.FileStatus = FileStatus.Failed;
                }
            });
        }
    }

    private void ResetParameters()
    {
        RemovePunctuation = false;
        RemoveShortWords = false;
        MinWordLength = 1;
        IsButtonEnabled = true;
        SelectedFilePaths = new ObservableCollection<FilePaths>();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(prop));
    }
}