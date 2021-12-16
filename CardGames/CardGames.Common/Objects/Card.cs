using CardGames.Common.Enums;

namespace CardGames.Common.Objects
{
    public sealed class Card
    {
        private Suits _Suit;
        private Values _Value;

        public Card(Suits cardSuit, Values cardValues)
        {
            _Suit = cardSuit;
            _Value = cardValues;
        }

        public Suits Suit => _Suit;
        public Values Value => _Value;
    }
}
