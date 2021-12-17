using CardGames.Common.Interfaces;
using CardGames.Common.Objects;
using CardGames.War.Models;

namespace CardGames.War
{
    public class WarGameManager : IGameManager
    {
        private readonly List<IPlayer> _Players;

        public WarGameManager(List<IPlayer> players)
        {
            _Players = players;
        }

        // Properties
        public int TurnCount { get; set; }

        // Methods
        public Task<bool> IsEndOfGame()
        {
            // Iterate to check for cards remaining
            foreach (var player in _Players)
            {
                if (player.Deck.Count <= 0)
                {
                    Console.WriteLine(player.Name + " is out of cards!");
                    player.IsOut = true;
                }
            }

            // Check exit condition
            var outPlayers = _Players.Where(x => x.IsOut).Count();
            var inPlayers = _Players.Where(x => !x.IsOut).Count();

            if (outPlayers - inPlayers == 1)
            {
                var winner = _Players.Where(x => !x.IsOut).First();

                Console.WriteLine($"Player {winner.Name} is our winner!");
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public Task ProcessPostTurn()
        {
            Console.WriteLine("\nPress any key for next turn...");
            Console.ReadKey();
            return Task.CompletedTask;
        }

        public Task ProcessPreTurn()
        {
            Console.Clear();
            return Task.CompletedTask;
        }

        public Task ProcessTurn()
        {
            // Increment Turn Count
            TurnCount++;

            // Pull out everyones card for the turn
            var cardTurn = _Players.Where(p => !p.IsOut)
                .Select(x => new WarHand
                {
                    PlayerNumber = x.PlayerNumber,
                    Name = x.Name,
                    Card = x.Deck.Draw()
                }).ToList();

            // Get the winning player from the top of the pile
            var winningHandPlayer = cardTurn.OrderByDescending(card => card?.Card?.Value)
                .First();

            // Draw the hand
            DrawHand(cardTurn, winningHandPlayer.PlayerNumber);

            // Enqueue the cards on the winners deck
            var winningPlayer = _Players.Where(p => p.PlayerNumber == winningHandPlayer.PlayerNumber)
                .First();
            winningPlayer.Deck.Add(cardTurn.Select(x => x.Card));

            // Return
            return Task.CompletedTask;
        }

        // Private Helpers
        private void DrawHand(IEnumerable<WarHand> hands, int winningPlayer)
        {
            Console.WriteLine($"Turn: {TurnCount}");
            // Draw the Hands
            foreach (var hand in hands)
            {
                Console.WriteLine($"\nPlayer {hand.Name}[{hand.PlayerNumber}] Card: {(hand?.PlayerNumber == winningPlayer ? " - WIN!" : string.Empty )}");
                Console.WriteLine(hand?.Card?.GetVisualization());
            }
        }
    }
}