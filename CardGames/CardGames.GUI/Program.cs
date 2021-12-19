using CardGames.Common.Services;
using Microsoft.Extensions.Logging;

Console.WriteLine("Starting the Card Game OS....\n");

// Init
ILoggerFactory _LoggerFactory = new LoggerFactory();
IPluginLoaderService _LoaderService = new PluginLoaderService(_LoggerFactory.CreateLogger<PluginLoaderService>());

// Set break flag
bool breakLoop = false;

// Initial Pass
while (!breakLoop)
{
    var rawSelection = await DisplayMenu();
    var menuSelection = int.MinValue;
    while (!await HandleIntegerConversions(rawSelection, out menuSelection, new KeyValuePair<int, int>(1, 4)))
    {
        Console.Write("\nInvalid Selection! Please enter a new selection: ");
        rawSelection = Console.ReadLine() ?? string.Empty;
    }

    // Switch to find the correct logic option
    switch (menuSelection)
    {
        case 1:
            await EnumerateAndExecuteAssembly();
            break;
        case 2:
            await LoadPluginAssembly();
            break;
        case 3:
            await _LoaderService.UnloadAllPlugins();
            break;
        case 4:
            // Shutdown the loop
            Console.Clear();
            Console.WriteLine("Exiting Application! Goodbye!");
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
    Console.Write("\nPlease make a selection: ");

    // Return
    return Task.FromResult(Console.ReadLine() ?? string.Empty);
}

/// <summary>
/// 
/// </summary>
async Task LoadPluginAssembly()
{
    // Clear
    Console.Clear();
    Console.Write("Enter the assembly fullpath that you intend on loading or load from the plugins directory [/plugins]: ");
    var fullPath = Console.ReadLine() ?? string.Empty;

    try
    {
        var result = false;
        // Check for path or plugin
        if (string.IsNullOrWhiteSpace(fullPath))
        {
            // Use the plugins directory
            var pluginsDirectory = $"{Directory.GetCurrentDirectory()}//plugins";

            // Load from the plugins directory
            result = await _LoaderService.LoadFromPluginFolder(pluginsDirectory);
        }
        else
        {
            // Error Checking
            while (!File.Exists(fullPath))
            {
                Console.Write("\nInvalid Path! Enter the assembly fullpath that you intend on loading: ");
                fullPath = Console.ReadLine();
            }

            // Load the Assembly
            result = await _LoaderService.LoadFromPath(fullPath);
        }

        if (!result)
            Console.WriteLine("\nAssembly was not found or failed to load.\n");
        else
            Console.WriteLine("\nAssembly successfully loaded!\n");
        // Finish
        return;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"\nError: {ex.Message}");
        return;
    }
}

/// <summary>
/// 
/// </summary>
async Task EnumerateAndExecuteAssembly()
{
    // Clear
    Console.Clear();
    // Check if there are assemblies
    if (_LoaderService.LoadedAssembliesCount == 0)
    {
        Console.WriteLine("\nNo Assemblies loaded!\n");
        return;
    }

    // Enumerate
    for (int i = 0; i < _LoaderService.LoadedAssembliesCount; i++)
    {
        // Get Assembly
        var assemblyName = _LoaderService.GetLoadedAssembliesNames.ElementAt(i);

        // Generate the list
        Console.WriteLine($"\n{i + 1}. {assemblyName.Name}");
    }

    // Select Game
    Console.Write("\nWhich game do you want to play? ");
    var selectionString = Console.ReadLine() ?? string.Empty;
    int gameSelection = -1;
    while (!await HandleIntegerConversions(selectionString, out gameSelection, new KeyValuePair<int, int>(1, _LoaderService.LoadedAssembliesCount)))
    {
        Console.Write("\nInvalid Selection! Please enter a new selection: ");
        selectionString = Console.ReadLine() ?? string.Empty;
    };

    // Select Player Count
    Console.Write("\nEnter number of Players? ");
    selectionString = Console.ReadLine() ?? string.Empty;
    int playerCount = -1;
    while (!await HandleIntegerConversions(selectionString, out playerCount, new KeyValuePair<int, int>(0, 5)))
    {
        Console.Write("\nInvalid Selection! Please enter a new selection: ");
        selectionString = Console.ReadLine() ?? string.Empty;
    }

    // Get Names
    List<string> playerNames = new List<string>(playerCount - 1);
    for (int i = 0; i < playerCount; i++)
    {
        Console.Write($"\nEnter a name for Player {i + 1}: ");
        var playerName = Console.ReadLine() ?? string.Empty;

        // Add it
        playerNames.Add(playerName);
    }

    // Call the Execute
    await _LoaderService.ExecutePlugin(_LoaderService.GetLoadedAssembliesNames.ElementAt(gameSelection - 1), playerNames);

    // Exit
    return;
}

Task<bool> HandleIntegerConversions(string selection, out int convertedValue, KeyValuePair<int, int>? bounds = null)
{
    // Init
    convertedValue = -1;
    // string null check
    if (string.IsNullOrEmpty(selection))
        return Task.FromResult(false);
    // Try convert to a number
    if (int.TryParse(selection, out int value))
    {
        // If bounds have been passed, check them
        if (bounds.HasValue)
        {
            if (value < bounds.Value.Key)
                return Task.FromResult(false);
            if (value > bounds.Value.Value)
                return Task.FromResult(false);
        }
        // Set the converted value
        convertedValue = value;
        return Task.FromResult(true);
    }
    else
        return Task.FromResult(false);

}