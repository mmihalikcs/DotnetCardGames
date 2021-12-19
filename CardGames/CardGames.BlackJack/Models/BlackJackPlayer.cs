using CardGames.Common.Interfaces;

namespace CardGames.BlackJack.Models
{
    public class BlackJackPlayer : IPlayer
    {
        public BlackJackPlayer(int number, string name)
        {
            _PlayerNumber = number;
            _Name = name;
        }

        // Interface
        private readonly int _PlayerNumber;
        public int PlayerNumber => _PlayerNumber;

        private readonly string _Name;
        public string Name { get => _Name; }

        public bool IsOut { get; set; }
    }
}
