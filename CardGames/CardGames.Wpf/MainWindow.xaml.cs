using CardGames.Wpf.ViewModel;
using System.Windows;

namespace CardGames.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel viewModel)
        {
            // DI Injection
            DataContext = viewModel;
            // Initialize WPF
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        // Properties


        // 
    }
}
