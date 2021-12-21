using CardGames.Wpf.ViewModel;
using System.Windows;
using System.Windows.Controls;

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

        /// <summary>
        /// Catch the windows closing event and utilize exit command in the MVM
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Use the exit command from the view model
            var command = (DataContext as MainWindowViewModel)?.ExitCommand;
            if (command != null && command.CanExecute(null))
                command.Execute(null);
        }
    }
}
