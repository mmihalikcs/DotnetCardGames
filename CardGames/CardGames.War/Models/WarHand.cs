using CardGames.Common.Objects;

namespace CardGames.War.Models
{
    public sealed class WarHand
    {
        public int PlayerNumber { get; set; }

        public string? Name { get; set; }

        public Card? Card { get; set; }
    }
}
