using CardGames.Common.Services;
using CardGames.Wpf.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace CardGames.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;
        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }
        private void ConfigureServices(ServiceCollection services)
        {
            // Logging
            services.AddLogging();
            // Add Services
            services.AddScoped<IPluginLoaderService, PluginLoaderService>();
            // Add the Main Window
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<MainWindow>();
        }
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow?.Show();
        }
    }
}
