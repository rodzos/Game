using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic
{
    public class PlayerState
    {
        public GameState Game { get; private set; }
        public Settings Settings => Game.Settings;
        public Random Random => Game.Random;

        public Dictionary<Zone, List<CardState>> Cards { get; private set; }
        public List<CardState> PutOff { get { return Cards[Zone.PutOff]; } }
        public List<CardState> Deck { get { return Cards[Zone.Deck]; } }
        public List<CardState> Hand { get { return Cards[Zone.Hand]; } }
        public List<CardState> Play { get { return Cards[Zone.Play]; } }
        public List<CardState> Pile { get { return Cards[Zone.Pile]; } }

        public int Health { get; private set; }
        public int MaxHealth { get; private set; }
        public int DeckCount { get; private set; }
        public bool Dead { get { return Health <= 0; } }

        public int Damage { get; private set; } = 0;
        public int Blocked { get; private set; } = 0;
        public CardState Played { get; private set; } = null;

        public PlayerState(GameState game, List<CardType> deck)
        {
            Game = game;
            Health = Settings.MaxHealth;
            MaxHealth = Settings.MaxHealth;
            Cards = new Dictionary<Zone, List<CardState>>()
            {
                { Zone.PutOff, Random.Shuffle(deck).Select(x => new CardState(x, this)).ToList() },
                { Zone.Deck, new List<CardState>() },
                { Zone.Hand, new List<CardState>() },
                { Zone.Play, new List<CardState>() },
                { Zone.Pile, new List<CardState>() }
            };
            DeckCount = 1;
        }

        public IEnumerable<CardState> AllCards()
        {
            return Cards.SelectMany(x => x.Value);
        }

        public Zone FindCard(CardState card)
        {
            foreach (var e in Cards)
                if (e.Value.Contains(card))
                    return e.Key;
            throw new ArgumentException();
        }

        public void MoveCard(CardState card, Zone target)
        {
            Zone from = FindCard(card);
            Cards[from].Remove(card);
            Cards[target].Add(card);
        }

        public void MoveCardFrom(CardState card, Zone source, Zone target)
        {
            if (FindCard(card) != source)
                throw new ArgumentException();
            MoveCard(card, target);
        }

        public bool ResetDeck()
        {
            if (Pile.Count == 0)
                return false;
            Deck.AddRange(Pile);
            Random.ShuffleInPlace(Deck);
            Pile.Clear();
            ++DeckCount;
            return true;
        }

        public bool EnsureDeckNotEmpty()
        {
            if (Deck.Count > 0)
                return true;
            return ResetDeck();
        }

        public void Discard(CardState card)
        {
            MoveCard(card, Zone.Pile);
        }

        public CardState DiscardMatching(Func<CardState, bool> predicate)
        {
            var card = Random.ChoiceOrDefault(Hand.Where(predicate));
            if (card != null)
                Discard(card);
            return card;
        }

        public CardState Discard()
        {
            if (Hand.Count == 0)
                return null;
            var card = Random.Choice(Hand);
            Discard(card);
            return card;
        }

        public void Draw(CardState card)
        {
            if (Hand.Count >= Settings.HandMax && Settings.StrictHandMax)
                Discard(card);
            else
                MoveCardFrom(card, Zone.Deck, Zone.Hand);
        }

        public CardState DrawMatching(Func<CardState, bool> predicate)
        {
            if (!EnsureDeckNotEmpty())
            {
                if (Settings.FailedDrawingFatigues)
                    ++DeckCount;
                return null;
            }
            if (DeckCount >= Settings.FatigueDeckCount)
                LoseMaxHealth(Settings.FatigueValue);
            CardState card = Deck.FirstOrDefault(predicate);
            if (card != null)
                Draw(card);
            return card;
        }

        public CardState Draw()
        {
            return DrawMatching(card => true);
        }

        public void LoseMaxHealth(int value)
        {
            MaxHealth -= value;
            CapHealthAbove(); // TODO
        }

        public void Attack(int value)
        {
            Health -= Math.Max(0, value - Math.Max(0, Blocked - Damage));
            Damage += value;
            if (Settings.CapHealthBelowDuringTurn)
                CapHealthBelow();
        }

        public void Block(int value)
        {
            Health += Math.Max(0, Math.Min(Damage - Blocked, value));
            Blocked += value;
            if (Settings.CapHealthAboveDuringTurn)
                CapHealthAbove();
        }

        public void Heal(int value)
        {
            Health += value;
            if (Settings.CapHealthAboveDuringTurn)
                CapHealthAbove();
        }

        public void MakeATurn(CardState card)
        {
            if (card != null && !Hand.Contains(card))
                throw new ArgumentException();
            Played = card;
            if (card != null)
                MoveCardFrom(card, Zone.Hand, Zone.Play);
        }

        public void CapHealthBelow()
        {
            if (Health < 0)
                Health = 0;
        }

        public void CapHealthAbove()
        {
            if (Health > MaxHealth)
                Health = MaxHealth;
        }

        public void FinishTurn()
        {
            CapHealthBelow();
            CapHealthAbove();
            Damage = 0;
            Blocked = 0;
            Played = null;
        }
    }

    public class ReadOnlyPlayerState
    {
        private PlayerState Player { get; set; }

        public Settings Settings => Player.Settings;
        public Random Random => Player.Random;
        public ReadOnlyDictionary<Zone, ReadOnlyCollection<CardState>> Cards {
            get
            {
                var dict = Player.Cards.ToDictionary(x => x.Key, x => new ReadOnlyCollection<CardState>(x.Value));
                return new ReadOnlyDictionary<Zone, ReadOnlyCollection<CardState>>(
                    new Dictionary<Zone, ReadOnlyCollection<CardState>>(dict));
            }
        }

        public ReadOnlyCollection<CardState> PutOff { get { return Cards[Zone.PutOff]; } }
        public ReadOnlyCollection<CardState> Deck { get { return Cards[Zone.Deck]; } }
        public ReadOnlyCollection<CardState> Hand { get { return Cards[Zone.Hand]; } }
        public ReadOnlyCollection<CardState> Play { get { return Cards[Zone.Play]; } }
        public ReadOnlyCollection<CardState> Pile { get { return Cards[Zone.Pile]; } }

        public int Health => Player.Health;
        public int MaxHealth => Player.MaxHealth;
        public int DeckCount => Player.DeckCount;
        public bool Dead => Player.Dead;

        public int Damage => Player.Damage;
        public int Blocked => Player.Blocked;
        public CardState Played => Player.Played;

        public ReadOnlyPlayerState(PlayerState player)
        {
            Player = player;
        }
    }
}
