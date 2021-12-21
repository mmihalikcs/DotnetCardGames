using CardGames.Common.Services;
using CardGames.Wpf.Models;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
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
            _PlayMenuItems = new ObservableCollection<MenuItemViewModel>();
        }

        // Property
        public IPluginLoaderService PluginService
        {
            get { return _PluginService; }
        }

        private ObservableCollection<MenuItemViewModel> _PlayMenuItems;
        public ObservableCollection<MenuItemViewModel> PlayMenuItems
        {
            get { return _PlayMenuItems; }
            private set { _PlayMenuItems = value; }
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
                    var pluginsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "plugins");
                    var result = await _PluginService.LoadFromPluginFolder(pluginsDirectory);
                    if (result)
                    {
                        // Clear the MenuItems
                        _PlayMenuItems.Clear();

                        // Load
                        foreach (var assembly in _PluginService.GetLoadedAssembliesNames)
                        {
                            _PlayMenuItems.Add(new MenuItemViewModel()
                            {
                                Header = assembly?.Name ?? String.Empty
                            });
                        }

                    }
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
                      return _PluginService.LoadedAssembliesCount > 0;
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
