using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGames.Common.Interfaces
{
    public interface IGameManager
    {
        public Task ProcessPreTurn();

        public Task ProcessTurn();

        public Task ProcessPostTurn();

        public Task<bool> IsEndOfGame();
    }
}
