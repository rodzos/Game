using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Logic.CardCollection;

namespace Game.Logic
{
    public class Deck : IList<CardType>
    {
        public static bool IsValid(IEnumerable<CardType> deck, GameRules gameRules, bool ignoreMinDeckSize = false)
        {
            var deckList = deck.ToList();
            if ((!ignoreMinDeckSize && deckList.Count < gameRules.MinDeckSize) || deckList.Count > gameRules.MaxDeckSize)
                return false;
            foreach (var card in deckList)
            {
                if (gameRules.MaxRarityInDeck.ContainsKey(card.Rarity) &&
                        deckList.Count(x => x == card) > gameRules.MaxRarityInDeck[card.Rarity])
                    return false;
            }
            foreach (var e in gameRules.MaxTagInDeck)
            {
                if (deckList.Count(x => x.Tags.Contains(e.Key)) > e.Value)
                    return false;
            }
            return true;
        }

        public static Deck MakeRandomDeck(ICardCollection cardCollection, GameRules gameRules, int deckSize, Random random)
        {
            var cards = cardCollection.GetCards().ToList();
            if (deckSize < gameRules.MinDeckSize || deckSize > gameRules.MaxDeckSize)
                throw new ArgumentException("Deck size is incorrect");
            var deck = new Deck(gameRules);
            while (deck.Count < deckSize)
            {
                var card = random.ChoiceOrDefault(cards.Where(x => deck.CanAddCard(x)));
                if (card == null)
                    throw new ArgumentException("Collection is not large enough");
                deck.Add(card);
            }
            return deck;
        }

        public static Deck MakeRandomDeck(ICardCollection cardCollection, GameRules gameRules, Random random)
        {
            return MakeRandomDeck(cardCollection, gameRules, gameRules.MinDeckSize, random);
        }

        private List<CardType> deck;
        public GameRules GameRules { get; private set; }

        public Deck(GameRules gameRules)
        {
            deck = new List<CardType>();
            GameRules = gameRules;
        }

        public Deck(IEnumerable<CardType> deck, GameRules gameRules)
        {
            this.deck = deck.ToList();
            GameRules = gameRules;
        }

        public Deck(Dictionary<CardType, int> deck, GameRules gameRules)
        {
            this.deck = new List<CardType>();
            foreach (var e in deck)
                for (int i = 0; i < e.Value; ++i)
                    this.deck.Add(e.Key);
            GameRules = gameRules;
        }

        public bool IsValid()
        {
            return IsValid(deck, GameRules);
        }

        public bool CanAddCard(CardType card)
        {
            return IsValid(deck.Concat(Enumerable.Repeat(card, 1)), GameRules, true);
        }

        public CardType this[int index] { get => deck[index]; set => deck[index] = value; }

        public int Count => deck.Count;

        public bool IsReadOnly => false;

        public void Add(CardType item)
        {
            deck.Add(item);
        }

        public void Clear()
        {
            deck.Clear();
        }

        public bool Contains(CardType item)
        {
            return deck.Contains(item);
        }

        public void CopyTo(CardType[] array, int arrayIndex)
        {
            deck.CopyTo(array, arrayIndex);
        }

        public IEnumerator<CardType> GetEnumerator()
        {
            return deck.GetEnumerator();
        }

        public int IndexOf(CardType item)
        {
            return deck.IndexOf(item);
        }

        public void Insert(int index, CardType item)
        {
            deck.Insert(index, item);
        }

        public bool Remove(CardType item)
        {
            return deck.Remove(item);
        }

        public void RemoveAt(int index)
        {
            deck.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return deck.GetEnumerator();
        }
    }
}
