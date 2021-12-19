using CardGames.Common.Interfaces;
using CardGames.Common.Objects;

namespace CardGames.War.Models
{
    public class WarPlayer : IPlayer
    {
        public WarPlayer(int number, string name)
        {
            _PlayerNumber = number;
            _Name = name;

            // Instantiate Deck Auto Property
            Deck = new DeckOfCards();
        }

        // Interface
        private readonly int _PlayerNumber;
        public int PlayerNumber => _PlayerNumber;

        private readonly string _Name;
        public string Name { get => _Name; }

        public bool IsOut { get; set; }

        // Extensions
        public DeckOfCards Deck { get; set; }
    }
}
