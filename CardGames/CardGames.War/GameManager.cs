using CardGames.Common.Interfaces;

namespace CardGames.War
{
    public class WarGameManager : IGameManager
    {
        public WarGameManager()
        {

        }

        public int TurnCount { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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