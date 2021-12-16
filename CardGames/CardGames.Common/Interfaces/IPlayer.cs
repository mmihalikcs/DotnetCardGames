using CardGames.Common.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGames.Common.Interfaces
{
    public interface IPlayer
    {
        public string Name { get; set; }
        
        public DeckOfCards Deck { get; set; }
    }
}
