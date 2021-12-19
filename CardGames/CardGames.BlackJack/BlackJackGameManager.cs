using CardGames.BlackJack.Models;
using CardGames.Common.Interfaces;
using CardGames.Common.Objects;

namespace CardGames.BlackJack
{
    public class BlackJackGameManager : IGameManager
    {
        private readonly List<IPlayer> _Players;
        private readonly BlackJackOptions _Options;
        private DeckOfCards? _Deck;

        public BlackJackGameManager(List<IPlayer> players, BlackJackOptions? options = null)
        {
            _Players = players;
            _Options = options ?? new BlackJackOptions()
            {
                NumberOfHands = 10
            };
            // Add the dealer
            _Players.Add(new BlackJackPlayer(0, "Dealer"));
            _Players = _Players.OrderBy(o => o.PlayerNumber).ToList();
        }

        // Properties
        public int TurnCount { get; set; }

        // Methods
        public Task<bool> IsEndOfGame()
        {
            if (TurnCount >= _Options.NumberOfHands)
                return Task.FromResult(true);
            // Default Return
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
            // Clear the screen
            Console.Clear();
            // Instantiate a new deck
            _Deck = new DeckOfCards();
            _Deck.Shuffle();
            // End Pre Turns
            return Task.CompletedTask;
        }

        public Task ProcessTurn()
        {
            // Increment TurnCounter
            TurnCount++;

            // Draw cards off the common deck 


            return Task.CompletedTask;
        }

        // Events
        public Task HandleUIEvent()
        {

            return Task.CompletedTask;
        }
    }
}
