using CardGames.Common.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace CardGames.Common.Services
{
    public interface IPluginLoaderService
    {
        // Properties
        public int LoadedAssembliesCount { get; }
        public IEnumerable<AssemblyName> GetLoadedAssembliesNames { get; }

        // Methods
        public Task<bool> LoadFromPluginFolder(string folderPath);
        public Task<bool> LoadFromPath(string filePath);
        public Task<bool> UnloadAllPlugins();
        public Task<bool> ExecutePlugin(AssemblyName assembly, List<string> playerNames);
    }

    public sealed class PluginLoaderService : IPluginLoaderService
    {
        private readonly AssemblyLoadContext _AssemblyLoader;
        private readonly ILogger<PluginLoaderService> _Logger;

        public PluginLoaderService(ILogger<PluginLoaderService> logger)
        {
            // Bind Logger
            _Logger = logger;
            // Instantiate assembly loader
            _AssemblyLoader = new AssemblyLoadContext("PluginLoader", true);
        }

        // Properties
        public int LoadedAssembliesCount => _AssemblyLoader.Assemblies.Count();
        public IEnumerable<AssemblyName> GetLoadedAssembliesNames
        {
            get
            {
                foreach (var assembly in _AssemblyLoader.Assemblies)
                {
                    yield return assembly.GetName();
                }
            }
        }

        // Public
        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public Task<bool> LoadFromPluginFolder(string folderPath)
        {
            // Check Folder Arg
            if (string.IsNullOrWhiteSpace(folderPath))
            {
                _Logger.LogError("FilePath invalid or empty");
                return Task.FromResult(false);
            }

            // Check Folder 
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Enum Directories
            var assemblyFiles = Directory.EnumerateFiles(folderPath, "*.dll");
            try
            {
                var loadFlag = false;
                foreach (var assemblyFilePath in assemblyFiles)
                {
                    _AssemblyLoader.LoadFromAssemblyPath(assemblyFilePath);
                    // Set the load flag
                    loadFlag = true;
                }
                return Task.FromResult(loadFlag);
            }
            catch (Exception ex)
            {
                // Log
                _Logger.LogError($"Failure to load assembly: {ex.Message}");
                // Return
                return Task.FromResult(false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public Task<bool> LoadFromPath(string filePath)
        {
            try
            {
                _AssemblyLoader.LoadFromAssemblyPath(filePath);
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                // Log
                _Logger.LogError($"Failure to load assembly: {ex.Message}");
                // Return
                return Task.FromResult(false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<bool> UnloadAllPlugins()
        {
            try
            {
                _AssemblyLoader.Unload();
                return Task.FromResult(true);
            }
            catch (InvalidOperationException ex)
            {
                _Logger.LogError($"Exception unloading plugins: {ex.Message}");
                // Default Return
                return Task.FromResult(false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ExecutePlugin(AssemblyName assembly, List<string> playerNames)
        {
            // Get Assembly
            var executingAssembly = _AssemblyLoader.Assemblies.Where(a => a.GetName().FullName == assembly.FullName).FirstOrDefault();
            if (executingAssembly == null)
                throw new NullReferenceException("assembly");

            // Get Game Manager interface type
            var gameManagerType = executingAssembly?.GetTypes()
                                .Where(t => !t.IsAbstract)
                                .Where(t => !t.IsInterface)
                                .Where(t => t.IsAssignableTo(typeof(IGameManager)))
                                .First();
            if (gameManagerType == null)
                throw new NullReferenceException("gameManager");

            // Get the Players interface type
            var playerType = executingAssembly?.GetTypes()
                                .Where(t => !t.IsAbstract)
                                .Where(t => !t.IsInterface)
                                .Where(t => t.IsAssignableTo(typeof(IPlayer)))
                                .First();
            if (playerType == null)
                throw new NullReferenceException("playerType");

            // Create the player args
            List<IPlayer> playerList = new List<IPlayer>();
            for (int i = 0; i < playerNames.Count; i++)
            {
                // Create a player
                var player = Activator.CreateInstance(playerType, i+1, playerNames[i]) as IPlayer;
                if (player == null)
                    throw new Exception("Failed to create class 'IPlayer'");
                // Add to list
                playerList.Add(player);
            }

            // Activate it
            var gameManagerInstance = Activator.CreateInstance(gameManagerType, playerList) as IGameManager;
            if (gameManagerInstance == null)
                throw new NullReferenceException("gameManagerInstance");

            // Play the game
            while (!(await gameManagerInstance.IsEndOfGame()))
            {
                await gameManagerInstance.ProcessPreTurn();
                await gameManagerInstance.ProcessTurn();
                await gameManagerInstance.ProcessPostTurn();
            }

            // Default Return
            return true;
        }
    }
}
