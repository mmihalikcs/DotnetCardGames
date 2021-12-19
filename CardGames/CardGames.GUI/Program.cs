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
        var z = _LoaderService.LoadFromPath(fullPath);
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
async Task EnumerateAndExecuteAssembly()
{
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
    await HandleIntegerConversions(selectionString, out int gameSelection);

    // Select Player Count
    Console.Write("\nEnter number of Players? ");
    selectionString = Console.ReadLine() ?? string.Empty;
    await HandleIntegerConversions(selectionString, out int playerCount);

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