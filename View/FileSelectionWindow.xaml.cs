using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using ViewModel;

namespace cadwiseTest;

public partial class FileSelectionWindow
{
    private bool _areFilesSelected;
    
    
    public FileSelectionWindow(object data)
    {
        InitializeComponent();
        DataContext = data;
    }

    private void OnOpenFileDialogClick(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "Текстовые файлы (*.txt)|*.txt",
            Multiselect = false,
            Title = "Выберите файл для обработки"
        };
        var saveFileDialog = new SaveFileDialog
        {
            Filter = "Текстовые файлы (*.txt)|*.txt",
            Title = "Выберите выходной файл"
        };
        if (openFileDialog.ShowDialog() != true) return;
        
        var inputFilePath = openFileDialog.FileName;

        _areFilesSelected = true;

        if (saveFileDialog.ShowDialog() != true) return;
        
        var outputFilePath = saveFileDialog.FileName;
        
        if (string.Equals(inputFilePath, outputFilePath))
        {
            MessageBox.Show("Входной и выходной файлы не должны совпадать.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        var filePaths = new FilePaths(inputFilePath, outputFilePath);

        var viewModel = DataContext as MainViewModel;
        if (viewModel?.SelectFilesCommand.CanExecute(filePaths) == true)
        {
            viewModel.SelectFilesCommand.Execute(filePaths);
        }
    }
    
    private void OnStartProcessingFileClick(object sender, RoutedEventArgs e)
    {
        if (_areFilesSelected)
        {
            DialogResult = true;
            Close();
            
            var viewModel = DataContext as MainViewModel;
            if (viewModel?.StartProcessingCommand.CanExecute(null) == true)
            {
                viewModel.StartProcessingCommand.Execute(null);
            } 
            
        }
        else
        {
            MessageBox.Show("Пожалуйста, выберите файл для обработки.");
        }
    }

    private void WordDeleteCheckBox_Checked(object sender, RoutedEventArgs e)
    {
        WordDeleteLength.IsEnabled = true;
        WordDeleteLength.Opacity = 1.0;
        WordDeleteText.IsEnabled = true;
        WordDeleteText.Opacity = 1.0;
    }

    private void WordDeleteCheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
        WordDeleteLength.IsEnabled = false;
        WordDeleteLength.Opacity = 0.5;
        WordDeleteText.IsEnabled = false;
        WordDeleteText.Opacity = 0.5;
    }
    
    private void NumberValidation(object sender, TextChangedEventArgs e)
    {
        TextBox textBox = sender as TextBox;
        if (textBox == null)
            return;
        string text = textBox.Text;
        Regex regex = new Regex(@"^(0|[1-9][0-9]{0,3})$");
        if (!regex.IsMatch(text))
        {
            textBox.Text = "";
            textBox.CaretIndex = 1; 
        }
    }

    private void OnCancelClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}