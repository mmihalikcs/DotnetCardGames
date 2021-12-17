using CardGames.Common.Enums;
using System.Text;

namespace CardGames.Common.Objects
{
    public sealed class Card
    {
        private Suits _Suit;
        private int _Value;

        public Card(Suits cardSuit, int cardValue)
        {
            _Suit = cardSuit;
            _Value = cardValue;
        }

        public Suits Suit => _Suit;
        public int Value => _Value;

        // Methods
        public string GetVisualization()
        {
            var stringBuilder = new StringBuilder();
            // Calculations
            var extraSpace = GetCardImage().Length < 2 ? " " : "";

            // Base Card
            stringBuilder.AppendLine("┌─────────┐");
            stringBuilder.AppendLine($"│{GetCardImage()}{extraSpace}       │");
            stringBuilder.AppendLine("│         │");
            stringBuilder.AppendLine("│         │");
            stringBuilder.AppendLine($"│    {GetSuitImage()}    │");
            stringBuilder.AppendLine("│         │");
            stringBuilder.AppendLine("│         │");
            stringBuilder.AppendLine($"│       {extraSpace}{GetCardImage()}│");
            stringBuilder.AppendLine("└─────────┘");
            // Return
            return stringBuilder.ToString();
        }

        // Private
        private string GetCardImage() => _Value switch
        {
            11 => "J",
            12 => "Q",
            13 => "K",
            14 => "A",
            _ => _Value.ToString()
        };

        private char GetSuitImage() => Suit switch
        {
            Suits.Diamonds => '♦',
            Suits.Hearts => '♥',
            Suits.Clubs => '♣',
            Suits.Spades => '♠'
        };
    }
}
