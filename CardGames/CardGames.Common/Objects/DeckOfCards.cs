using CardGames.Common.Enums;

namespace CardGames.Common.Objects
{
    public sealed class DeckOfCards
    {
        private Queue<Card> _Deck;
        private Random _Random;

        public DeckOfCards()
        {
            _Deck = LoadDeck();
            _Random = new Random();
        }

        public int Count => _Deck.Count();

        #region Public Methods
        /// <summary>
        /// Randomly shuffles the deck
        /// Fisher-Yates algorithm
        /// </summary>
        public void Shuffle()
        {
            // Save off the queue as a list
            var cardList = _Deck.ToList();
            // Shuffle
            for (var i = 0; i < cardList.Count; i++)
                Swap(cardList, i, _Random.Next(i, cardList.Count));
            // Reset the queue
            _Deck = new Queue<Card>(cardList);
        }

        /// <summary>
        /// Peeks the top card on the deck.
        /// Does not remove.
        /// </summary>
        /// <returns></returns>
        public Card Peek()
        {
            return _Deck.Peek();
        }

        /// <summary>
        /// Draws a single card from the top of the deck.
        /// Removes the card from the deck.
        /// </summary>
        /// <returns></returns>
        public Card Draw()
        {
            return _Deck.Dequeue();
        }

        /// <summary>
        /// Draws n cards from the top of the deck.
        /// Removes the cards from the deck.
        /// </summary>
        /// <param name="numberOfCards"></param>
        /// <returns></returns>
        public IEnumerable<Card> Draw(int numberOfCards)
        {
            // Return Collection
            List<Card> cards = new List<Card>();
            // Logic
            for (var i = 0; i < numberOfCards; i++)
                cards.Add(_Deck.Dequeue());
            // Return
            return cards.AsEnumerable();
        }

        /// <summary>
        /// Add a card to the deck
        /// </summary>
        /// <param name="card"></param>
        public void Add(Card card)
        {
            _Deck.Enqueue(card);
        }

        /// <summary>
        /// Add several cards to the deck
        /// </summary>
        /// <param name="cards"></param>
        public void Add(IEnumerable<Card> cards)
        {
            foreach (var card in cards)
                _Deck.Enqueue(card);
        }

        #endregion

        #region Private Helpers
        /// <summary>
        /// Standard deck of cards has 52 cards. 
        /// 2-10, Jack, Queen, King, Ace
        /// </summary>
        /// <returns></returns>
        Queue<Card> LoadDeck()
        {
            Queue<Card> tmpDeck = new Queue<Card>();
            // Add Suite Ranges
            foreach (var suit in Enum.GetValues(typeof(Suits)).Cast<Suits>())
            {
                // Generate an enumerable list
                var range = Enumerable.Range(2, 13).Select(i => new Card(suit, i));
                foreach (var card in range)
                {
                    tmpDeck.Enqueue(card);
                }
            }
            return tmpDeck;
        }

        void Swap(List<Card> cards, int i, int j)
        {
            var temp = cards[i];
            cards[i] = cards[j];
            cards[j] = temp;
        }
        #endregion
    }
}
