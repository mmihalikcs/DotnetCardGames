// See https://aka.ms/new-console-template for more information
using CardGames.Common.Interfaces;
using CardGames.Common.Objects;
using CardGames.War;

Console.WriteLine("Starting the Card Game OS....");

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
        case 1:
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
        case 2:

            break;
        case 3:

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

Task<string> DisplayMenu()
{
    Console.WriteLine("\t1. Play Game");
    Console.WriteLine("\t2. Load Game");
    Console.WriteLine("\t3. Unload Game");
    Console.WriteLine("\t4. Quit");
    Console.Write("\n\nPlease make a selection: ");

    // Return
    return Task.FromResult(Console.ReadLine() ?? string.Empty);
}