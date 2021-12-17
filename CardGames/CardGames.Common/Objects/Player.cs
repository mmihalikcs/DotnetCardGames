using CardGames.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGames.Common.Objects
{
    public sealed class Player : IPlayer
    {
        public Player(int number, string name)
        {
            _PlayerNumber = number;
            _Name = name;
            
            // Instantiate Deck Auto Property
            Deck = new DeckOfCards();
        }

        private readonly int _PlayerNumber;
        public int PlayerNumber => _PlayerNumber;

        private readonly string _Name;
        public string Name { get => _Name; }

        // Auto Props
        public DeckOfCards Deck { get; set; }
        public bool IsOut { get; set; }
    }
}
