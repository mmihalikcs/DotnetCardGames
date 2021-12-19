namespace CardGames.Common.Interfaces
{
    public interface IPlayer
    {
        public int PlayerNumber { get; }

        public string Name { get; }

        public bool IsOut { get; set; }
    }
}
