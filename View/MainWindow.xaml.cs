using System.Windows;
using ViewModel;

namespace cadwiseTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
        
        public void OnOpenFileSelection(object sender, RoutedEventArgs e)
        {
            FileSelectionWindow fileSettingsWindow = new FileSelectionWindow(DataContext);
            fileSettingsWindow.ShowDialog();
        }
        
    }
}