// See https://aka.ms/new-console-template for more information
using CardGames.Common.Interfaces;
using CardGames.Common.Objects;
using CardGames.War;
using System.Runtime.Loader;

Console.WriteLine("Starting the Card Game OS....\n");

// Init
var assemblyLoader = new AssemblyLoadContext("PluginLoader", true);

// Set break flag
bool breakLoop = false;

// Initial Pass
while (!breakLoop)
{
    var rawSelection = await DisplayMenu();
    while (!int.TryParse(rawSelection, out int selection) ||
        (selection < 0 || selection > 4))
    {
        Console.Write("\nInvalid Selection! Please enter a new selection: ");
        rawSelection = Console.ReadLine();
    }

    // Switch to find the correct logic option
    switch (Convert.ToInt32(rawSelection))
    {
        case 0:
            // Starting War
            Console.WriteLine("\nStarting War!\n\n");
            Console.Write("How many players? ");
            var gameSelection = Console.ReadLine();

            // Specify number of players
            var gmeSelection = Convert.ToInt32(gameSelection);
            var playerCounter = 1;
            List<IPlayer> players = new List<IPlayer>();
            for (int i = 0; i < gmeSelection; i++)
            {
                Console.Write($"What is the name of Player {i + 1}? ");
                var name = Console.ReadLine() ?? string.Empty;

                // Create a player
                var player = new Player(playerCounter, name);
                player.Deck.Shuffle();

                // Add to list
                players.Add(player);

                // Increment Counter
                playerCounter++;
            }

            IGameManager gameManager = new WarGameManager(players);
            while (!(await gameManager.IsEndOfGame()))
            {
                await gameManager.ProcessPreTurn();
                await gameManager.ProcessTurn();
                await gameManager.ProcessPostTurn();
            }
            break;
        case 1:
            if (assemblyLoader.Assemblies.Count() == 0)
            {
                Console.WriteLine("\nNo Assemblies loaded!\n");
                break;
            }

            // Main Function
            await EnumeratePluginAssemblies();

            break;
        case 2:
            await LoadPluginAssembly();
            break;
        case 3:
            await UnloadAllPlugins();
            break;
        case 4:
            // Shutdown the loop
            Console.WriteLine("\nExiting Application! Goodbye!");
            breakLoop = true;
            break;
    }
}

// Graceful exit
Environment.Exit(0);


/*
 * Private Helpers
 */

/// <summary>
/// 
/// </summary>
Task<string> DisplayMenu()
{
    Console.WriteLine("1. Play Game");
    Console.WriteLine("2. Load Game");
    Console.WriteLine("3. Unload Game");
    Console.WriteLine("4. Quit");
    Console.Write("\n\nPlease make a selection: ");

    // Return
    return Task.FromResult(Console.ReadLine() ?? string.Empty);
}

/// <summary>
/// 
/// </summary>
Task LoadPluginAssembly()
{
    // Clear
    Console.Clear();
    Console.Write("Enter the assembly fullpath that you intend on loading: ");
    var fullPath = Console.ReadLine();

    // Error Checking
    while (!File.Exists(fullPath))
    {
        Console.Write("\nInvalid Path! Enter the assembly fullpath that you intend on loading: ");
        fullPath = Console.ReadLine();
    }

    try
    {
        // Load the Assembly
        assemblyLoader.LoadFromAssemblyPath(fullPath);
    }
    catch (FileLoadException ex)
    {
        throw ex;
    }

    // Report Success
    Console.WriteLine("\nAssembly Successfully Loaded!\n");
    return Task.CompletedTask;
}

/// <summary>
/// 
/// </summary>
Task EnumeratePluginAssemblies()
{
    Console.WriteLine("\nDoing Stuff and Things\n");


    return Task.CompletedTask;
}

/// <summary>
/// 
/// </summary>
Task UnloadAllPlugins()
{
    // Clear the console
    Console.Clear();
    // Report unload
    Console.WriteLine("Unloading all plugins...");
    // Perform the unload
    try
    {
        assemblyLoader?.Unload();
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine(ex.Message);
        throw;
    }

    // Report Success
    Console.WriteLine("\nUnload Complete!\n");
    return Task.CompletedTask;
}