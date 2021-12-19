using CardGames.Common.Interfaces;

namespace CardGames.BlackJack
{
    public class BlackJackGameManager : IGameManager
    {
        private readonly List<IPlayer> _Players;

        public BlackJackGameManager(List<IPlayer> players)
        {
            _Players = players;
        }

        // Properties
        public int TurnCount { get; set; }

        // Methods
        public Task<bool> IsEndOfGame()
        {
            throw new NotImplementedException();
        }

        public Task ProcessPostTurn()
        {
            throw new NotImplementedException();
        }

        public Task ProcessPreTurn()
        {
            throw new NotImplementedException();
        }

        public Task ProcessTurn()
        {
            throw new NotImplementedException();
        }
    }
}
