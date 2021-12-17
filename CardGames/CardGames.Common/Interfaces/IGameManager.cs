using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGames.Common.Interfaces
{
    public interface IGameManager
    {
        // Properties
        public int TurnCount { get; protected set; }

        // Methods
        public Task ProcessPreTurn();

        public Task ProcessTurn();

        public Task ProcessPostTurn();

        public Task<bool> IsEndOfGame();
    }
}
