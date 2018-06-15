using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Game.Logic;

namespace GameTests.LogicTests
{
    [TestFixture]
    class LogicTests
    {
        private readonly GameRules rules = new GameRules();
        private readonly Func<EffectMaker> makeGlobalEffects = () => new Game.Logic.EffectMakers.GlobalEffects();

        public static void InstantlyMakeTurn(GameState game, Phase phase)
        {
            foreach (var effect in game.MakeTurn(phase))
                effect.Call();
        }

        private readonly CardType attack5 = new CardType("Атака 5", "Атака 5", Rarity.Common,
            () => new Game.Logic.EffectMakers.SequentialEffectMaker(
                Game.Logic.EffectMakers.SequentialEffectMakerItem.StartInDeck(),
                Game.Logic.EffectMakers.SequentialEffectMakerItem.PlayEffect(false, x => new Game.Logic.Effects.AttackEffect(5)),
                Game.Logic.EffectMakers.SequentialEffectMakerItem.End()
            ));

        [Test]
        public void TestEmptyDecks()
        {
            var game = new GameState(new List<Deck> { new Deck(rules), new Deck(rules) }, makeGlobalEffects, rules);
        }

        [Test]
        public void TestNotTwoDecks()
        {
            var deckCounts = new int[] { 0, 1, 3 };
            foreach (var deckCount in deckCounts)
            {
                var decks = new List<Deck>();
                for (int i = 0; i < deckCount; ++i)
                    decks.Add(new Deck(rules));
                Assert.Throws<ArgumentException>(() => new GameState(decks, makeGlobalEffects, rules));
            }
        }

        [Test]
        public void TestDeckValidation()
        {
            var commonCards = new List<CardType>();
            for (int i = 0; i < 21; ++i)
                commonCards.Add(new CardType($"Common card {i + 1}", "", Rarity.Common, null, new List<Tag>()));
            var rareCard = new CardType("Rare card", "", Rarity.Rare, null, new List<Tag>());
            var rareCard2 = new CardType("Rare card 2", "", Rarity.Rare, null, new List<Tag>());
            var autoCard = new CardType("Auto card", "", Rarity.Common, null, new List<Tag> { Tag.Auto });
            var autoCard2 = new CardType("Auto card 2", "", Rarity.Common, null, new List<Tag> { Tag.Auto });
            var superCard = new CardType("Super card", "", Rarity.Common, null, new List<Tag> { Tag.Super });
            var superCard2 = new CardType("Super card 2", "", Rarity.Common, null, new List<Tag> { Tag.Super });

            Assert.IsFalse(new Deck(rules).IsValid());
            Assert.IsFalse(new Deck(commonCards.Take(14), rules).IsValid());
            Assert.IsTrue(new Deck(commonCards.Take(15), rules).IsValid());
            Assert.IsTrue(new Deck(commonCards.Take(20), rules).IsValid());
            Assert.IsFalse(new Deck(commonCards, rules).IsValid());

            Assert.IsTrue(new Deck(Enumerable.Range(0, 20).Select(x => commonCards[x / 2]), rules).IsValid());
            Assert.IsFalse(new Deck(commonCards.Take(12).Concat(Enumerable.Repeat(commonCards[0], 3)), rules).IsValid());

            Assert.IsTrue(new Deck(commonCards.Take(14).Concat(new CardType[] { rareCard }), rules).IsValid());
            Assert.IsFalse(new Deck(commonCards.Take(13).Concat(new CardType[] { rareCard, rareCard }), rules).IsValid());
            Assert.IsTrue(new Deck(commonCards.Take(13).Concat(new CardType[] { rareCard, rareCard2 }), rules).IsValid());

            Assert.IsTrue(new Deck(commonCards.Take(13).Concat(new CardType[] { autoCard, autoCard2 }), rules).IsValid());
            Assert.IsTrue(new Deck(commonCards.Take(13).Concat(new CardType[] { autoCard, autoCard }), rules).IsValid());
            Assert.IsFalse(new Deck(commonCards.Take(12).Concat(new CardType[] { autoCard, autoCard, autoCard }), rules).IsValid());
            Assert.IsFalse(new Deck(commonCards.Take(12).Concat(new CardType[] { autoCard, autoCard, autoCard2 }), rules).IsValid());

            Assert.IsTrue(new Deck(commonCards.Take(14).Concat(new CardType[] { superCard, autoCard2 }), rules).IsValid());
            Assert.IsFalse(new Deck(commonCards.Take(13).Concat(new CardType[] { superCard, superCard }), rules).IsValid());
            Assert.IsFalse(new Deck(commonCards.Take(13).Concat(new CardType[] { superCard, superCard2 }), rules).IsValid());
        }

        [Test]
        public void TestStartOfGame()
        {
            var deck = new Deck(Enumerable.Repeat(attack5, 15), rules);
            var game = new GameState(new List<Deck> { deck, deck }, makeGlobalEffects, rules);
            InstantlyMakeTurn(game, Phase.StartOfGame);
            Assert.AreEqual(2, game.Players.Count);
            for (int i = 0; i < game.Players.Count; ++i)
            {
                Assert.AreEqual(15, game.Players[i].Deck.Count);
                Assert.AreEqual(0, game.Players[i].PutOff.Count);
                Assert.AreEqual(0, game.Players[i].Hand.Count);
                Assert.AreEqual(0, game.Players[i].Play.Count);
                Assert.AreEqual(0, game.Players[i].Pile.Count);
            }
        }

        [Test]
        public void TestDrawFirstCards()
        {
            var deck = new Deck(Enumerable.Repeat(attack5, 15), rules);
            var game = new GameState(new List<Deck> { deck, deck }, makeGlobalEffects, rules);
            InstantlyMakeTurn(game, Phase.StartOfGame);
            InstantlyMakeTurn(game, Phase.Choose);
            for (int i = 0; i < game.Players.Count; ++i)
            {
                Assert.AreEqual(15 - rules.StartingHand, game.Players[i].Deck.Count);
                Assert.AreEqual(rules.StartingHand, game.Players[i].Hand.Count);
                Assert.AreEqual(0, game.Players[i].PutOff.Count);
                Assert.AreEqual(0, game.Players[i].Play.Count);
                Assert.AreEqual(0, game.Players[i].Pile.Count);
            }
        }

        [Test]
        public void TestMakeATurn()
        {
            var deck = new Deck(Enumerable.Repeat(attack5, 15), rules);
            var game = new GameState(new List<Deck> { deck, deck }, makeGlobalEffects, rules);
            InstantlyMakeTurn(game, Phase.StartOfGame);
            InstantlyMakeTurn(game, Phase.Choose);
            game.Players[0].MakeATurn(game.Players[0].Hand[0]);
            InstantlyMakeTurn(game, Phase.Play);
            InstantlyMakeTurn(game, Phase.End);

            for (int i = 0; i < game.Players.Count; ++i)
            {
                Assert.AreEqual(15 - rules.StartingHand, game.Players[i].Deck.Count);
                Assert.AreEqual(rules.StartingHand - (i == 0 ? 1 : 0), game.Players[i].Hand.Count);
                Assert.AreEqual(0, game.Players[i].PutOff.Count);
                Assert.AreEqual(0, game.Players[i].Play.Count);
                Assert.AreEqual(i == 0 ? 1 : 0, game.Players[i].Pile.Count);
            }
        }
    }
}
