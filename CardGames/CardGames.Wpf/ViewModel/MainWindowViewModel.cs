using CardGames.Common.Services;
using Prism.Commands;
using System;
using System.IO;
using System.Windows.Input;

namespace CardGames.Wpf.ViewModel
{
    public class MainWindowViewModel
    {
        private readonly IPluginLoaderService _PluginService;

        public MainWindowViewModel(IPluginLoaderService pluginService)
        {
            _PluginService = pluginService;
        }

        #region Commands

        /// <summary>
        /// Used to load assemblies
        /// </summary>
        public ICommand LoadCommand
        {
            get
            {
                return new DelegateCommand<object>(async (object obj) =>
                {
                    var pluginsDirectory = $"{Directory.GetCurrentDirectory}//plugins";
                    var result = await _PluginService.LoadFromPluginFolder(pluginsDirectory);
                },
                  (object obj) =>
                  {
                      return true;
                  });
            }
        }

        /// <summary>
        /// Used to load assemblies
        /// </summary>
        public ICommand UnloadCommand
        {
            get
            {
                return new DelegateCommand<object>(async (object obj) =>
                {
                    var result = await _PluginService.UnloadAllPlugins();
                },
                  (object obj) =>
                  {
                      return true;
                  });
            }
        }

        /// <summary>
        /// Used to gracefully exit the application
        /// </summary>
        public ICommand AboutCommand
        {
            get
            {
                return new DelegateCommand<object>((object obj) =>
                {
                    return;
                },
                  (object obj) =>
                  {
                      return false;
                  });
            }
        }

        /// <summary>
        /// Used to gracefully exit the application
        /// </summary>
        public ICommand ExitCommand
        {
            get
            {
                return new DelegateCommand<object>((object obj) =>
                {
                    Environment.Exit(0);
                },
                  (object obj) =>
                  {
                      return true;
                  });
            }
        }

        #endregion Commands

    }
}
