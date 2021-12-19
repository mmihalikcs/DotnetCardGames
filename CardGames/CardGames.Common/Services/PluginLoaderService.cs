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
        public Task ExecutePlugin(AssemblyName name);
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

            return Task.FromResult(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public Task<bool> LoadFromPath(string filePath)
        {

            return Task.FromResult(false);
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
        public Task ExecutePlugin(AssemblyName name)
        {

            return Task.CompletedTask;
        }

    }
}
