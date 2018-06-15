using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Logic;
using Game.Logic.CardCollection;
using NUnit.Framework;

namespace GameTests.LogicTests
{
    [TestFixture]
    class CardCollectionTests
    {
        private readonly GameRules rules = new GameRules();
        private readonly Func<EffectMaker> makeGlobalEffects = () => new Game.Logic.EffectMakers.GlobalEffects();

        public static void InstantlyMakeTurn(GameState game, Phase phase)
        {
            foreach (var effect in game.MakeTurn(phase))
                effect.Call();
        }

        public GameState InitializeState(CardType myCard, CardType opponentCard, CardType fillerCard)
        {
            var chosenCards = new List<CardType> { myCard, opponentCard };
            var myDeck = new Deck(rules);
            var opponentDeck = new Deck(rules);
            for (int i = 0; i < rules.MinDeckSize; ++i)
            {
                myDeck.Add(i == 0 ? myCard : fillerCard);
                opponentDeck.Add(i == 0 ? opponentCard : fillerCard);
            }
            var game = new GameState(new List<Deck> { myDeck, opponentDeck }, makeGlobalEffects, rules);
            InstantlyMakeTurn(game, Phase.StartOfGame);

            for (int i = 0; i < game.Players.Count; ++i)
            {
                var card = game.Players[i].Deck.Find(x => x.Type == chosenCards[i]);
                if (card != null)
                {
                    game.Players[i].Deck.Remove(card);
                    game.Players[i].Deck.Insert(0, card);
                }
            }

            InstantlyMakeTurn(game, Phase.Choose);
            return game;
        }

        public GameState PlayTwoCards(CardType myCard, CardType opponentCard, CardType fillerCard)
        {
            var game = InitializeState(myCard, opponentCard, fillerCard);
            game.Players[0].MakeATurn(game.Players[0].Hand.Find(x => x.Type == myCard));
            game.Players[1].MakeATurn(game.Players[1].Hand.Find(x => x.Type == opponentCard));
            InstantlyMakeTurn(game, Phase.Play);
            InstantlyMakeTurn(game, Phase.End);
            return game;
        }

        public GameState PlayOneCard(CardType myCard, CardType fillerCard)
        {
            var game = InitializeState(myCard, fillerCard, fillerCard);
            game.Players[0].MakeATurn(game.Players[0].Hand.Find(x => x.Type == myCard));
            InstantlyMakeTurn(game, Phase.Play);
            InstantlyMakeTurn(game, Phase.End);
            return game;
        }

        public GameState PlayNoCards(CardType myCard, CardType fillerCard)
        {
            var game = InitializeState(myCard, fillerCard, fillerCard);
            InstantlyMakeTurn(game, Phase.Play);
            InstantlyMakeTurn(game, Phase.End);
            return game;
        }

        public void AssertIsOfType(CardType cardType, CardState card)
        {
            Assert.AreEqual(cardType, card.Type);
        }

        [Test]
        public void TestAttack()
        {
            var game = PlayOneCard(CardCollection.Attack, CardCollection.Block);
            Assert.AreEqual(100, game.Players[0].Health);
            Assert.AreEqual(85, game.Players[1].Health);
            Assert.AreEqual(2, game.Players[0].Hand.Count);
            Assert.AreEqual(1, game.Players[0].Pile.Count);
            AssertIsOfType(CardCollection.Attack, game.Players[0].Pile[0]);
        }

        [Test]
        public void TestHeal()
        {
            var game = InitializeState(CardCollection.Heal, CardCollection.Block, CardCollection.Block);
            game.Players[0].Attack(50);
            game.Players[0].MakeATurn(game.Players[0].Hand.Find(x => x.Type == CardCollection.Heal));
            InstantlyMakeTurn(game, Phase.Play);
            InstantlyMakeTurn(game, Phase.End);
            Assert.AreEqual(65, game.Players[0].Health);

            game = PlayTwoCards(CardCollection.Heal, CardCollection.Attack, CardCollection.Block);
            Assert.AreEqual(85, game.Players[0].Health);
        }

        [Test]
        public void TestBlock()
        {
            var game = InitializeState(CardCollection.Block, CardCollection.Attack, CardCollection.Heal);
            game.Players[0].MakeATurn(game.Players[0].Hand.Find(x => x.Type == CardCollection.Block));
            game.Players[1].MakeATurn(game.Players[1].Hand.Find(x => x.Type == CardCollection.Attack));
            game.Players[1].Play.Add(new CardState(CardCollection.Attack, game.Players[1]));
            InstantlyMakeTurn(game, Phase.Play);
            InstantlyMakeTurn(game, Phase.End);
            Assert.AreEqual(95, game.Players[0].Health);

            game = PlayTwoCards(CardCollection.Block, CardCollection.Attack, CardCollection.Heal);
            Assert.AreEqual(100, game.Players[0].Health);
        }

        [Test]
        public void TestBite()
        {
            var game = PlayNoCards(CardCollection.Bite, CardCollection.Block);
            Assert.AreEqual(90, game.Players[1].Health);
            Assert.AreEqual(3, game.Players[0].Hand.Count);
        }

        [Test]
        public void TestPointBlank()
        {
            var game = PlayOneCard(CardCollection.PointBlank, CardCollection.Block);
            Assert.AreEqual(90, game.Players[1].Health);

            game = InitializeState(CardCollection.PointBlank, CardCollection.Block, CardCollection.Block);
            game.Players[0].MakeATurn(game.Players[0].Hand.Find(x => x.Type == CardCollection.PointBlank));
            game.Players[0].Play.Add(new CardState(CardCollection.Attack, game.Players[0]));
            InstantlyMakeTurn(game, Phase.Play);
            InstantlyMakeTurn(game, Phase.End);
            Assert.AreEqual(65, game.Players[1].Health);
        }

        [Test]
        public void TestInSight()
        {
            var game = PlayOneCard(CardCollection.InSight, CardCollection.Block);
            Assert.AreEqual(100, game.Players[1].Health);
            Assert.AreEqual(1, game.Players[0].Play.Count);
            Assert.AreEqual(0, game.Players[0].Pile.Count);

            InstantlyMakeTurn(game, Phase.Choose);
            InstantlyMakeTurn(game, Phase.Play);
            InstantlyMakeTurn(game, Phase.End);
            Assert.AreEqual(85, game.Players[1].Health);
            Assert.AreEqual(0, game.Players[0].Play.Count);
            Assert.AreEqual(1, game.Players[0].Pile.Count);
        }

        [Test]
        public void TestBerserk()
        {
            var game = InitializeState(CardCollection.Berserk, CardCollection.Attack, CardCollection.Attack);
            InstantlyMakeTurn(game, Phase.StartOfGame);
            InstantlyMakeTurn(game, Phase.Choose);
            game.Players[0].Attack(10);
            game.Players[0].MakeATurn(game.Players[0].Hand.Find(x => x.Type == CardCollection.Berserk));
            InstantlyMakeTurn(game, Phase.Play);
            InstantlyMakeTurn(game, Phase.End);
            Assert.AreEqual(95, game.Players[0].Health);
            Assert.AreEqual(95, game.Players[1].Health);

            game = InitializeState(CardCollection.Berserk, CardCollection.Attack, CardCollection.Attack);
            InstantlyMakeTurn(game, Phase.StartOfGame);
            InstantlyMakeTurn(game, Phase.Choose);
            game.Players[0].Attack(25);
            game.Players[0].MakeATurn(game.Players[0].Hand.Find(x => x.Type == CardCollection.Berserk));
            game.Players[0].Play.Add(new CardState(CardCollection.Attack, game.Players[0]));
            InstantlyMakeTurn(game, Phase.Play);
            InstantlyMakeTurn(game, Phase.End);
            Assert.AreEqual(95, game.Players[0].Health);
            Assert.AreEqual(80, game.Players[1].Health);
        }

        [Test]
        public void TestMindReading()
        {
            var game = InitializeState(CardCollection.MindReading, CardCollection.Attack, CardCollection.Attack);
            game.Players[0].Attack(10);
            game.Players[0].MakeATurn(game.Players[0].Hand.Find(x => x.Type == CardCollection.MindReading));
            InstantlyMakeTurn(game, Phase.Play);
            InstantlyMakeTurn(game, Phase.End);
            Assert.AreEqual(95, game.Players[0].Health);
            Assert.AreEqual(80, game.Players[1].Health);
        }

        [Test, Timeout(5000)]
        public void TestMindReadingDoesNotLoopInfinitely()
        {
            var game = InitializeState(CardCollection.MindReading, CardCollection.MindReading, CardCollection.MindReading);
            game.Players[0].Attack(95);
            game.Players[0].MakeATurn(game.Players[0].Hand.Find(x => x.Type == CardCollection.MindReading));
            InstantlyMakeTurn(game, Phase.Play);
            InstantlyMakeTurn(game, Phase.End);
            Assert.AreEqual(100, game.Players[0].Health);
            Assert.IsTrue(game.Players[1].Dead);
        }

        [Test]
        public void TestStealth()
        {
            var game = PlayTwoCards(CardCollection.Stealth, CardCollection.Attack, CardCollection.Block);
            Assert.AreEqual(100, game.Players[0].Health);
            Assert.AreEqual(1, game.Players[0].Play.Count);
            Assert.AreEqual(0, game.Players[0].Pile.Count);
            InstantlyMakeTurn(game, Phase.Choose);
            InstantlyMakeTurn(game, Phase.Play);
            InstantlyMakeTurn(game, Phase.End);
            Assert.AreEqual(100, game.Players[1].Health);
            Assert.AreEqual(0, game.Players[0].Play.Count);
            Assert.AreEqual(1, game.Players[0].Pile.Count);
        }

        [Test]
        public void TestSacrifice()
        {
            var game = PlayOneCard(CardCollection.Sacrifice, CardCollection.Block);
            Assert.AreEqual(75, game.Players[0].Health);
            Assert.AreEqual(1, game.Players[0].Play.Count);
            Assert.AreEqual(0, game.Players[0].Pile.Count);
            InstantlyMakeTurn(game, Phase.Choose);
            game.Players[0].Attack(40);
            InstantlyMakeTurn(game, Phase.Play);
            InstantlyMakeTurn(game, Phase.End);
            Assert.AreEqual(85, game.Players[0].Health);
            Assert.AreEqual(0, game.Players[0].Play.Count);
            Assert.AreEqual(1, game.Players[0].Pile.Count);
        }

        [Test]
        public void TestRessurect()
        {
            var game = PlayTwoCards(CardCollection.Attack, CardCollection.Attack, CardCollection.Block);
            Assert.AreEqual(85, game.Players[0].Health);
            Assert.AreEqual(1, game.Players[0].Pile.Count);
            InstantlyMakeTurn(game, Phase.Choose);
            game.Players[0].Play.Add(new CardState(CardCollection.Ressurect, game.Players[0]));
            InstantlyMakeTurn(game, Phase.Play);
            InstantlyMakeTurn(game, Phase.End);
            Assert.AreEqual(95, game.Players[0].Health);
            Assert.AreEqual(1, game.Players[0].Pile.Count);
            AssertIsOfType(CardCollection.Ressurect, game.Players[0].Pile[0]);
            AssertIsOfType(CardCollection.Attack, game.Players[0].Hand.Last());
        }

        [Test]
        public void TestSpark()
        {
            var game = PlayOneCard(CardCollection.Spark, CardCollection.Attack);
            Assert.AreEqual(100, game.Players[1].Health);
            Assert.AreEqual(1, game.Players[0].Play.Count);
            Assert.AreEqual(0, game.Players[0].Pile.Count);

            InstantlyMakeTurn(game, Phase.Choose);
            InstantlyMakeTurn(game, Phase.Play);
            InstantlyMakeTurn(game, Phase.End);

            Assert.AreEqual(85, game.Players[1].Health);
            Assert.AreEqual(1, game.Players[0].Play.Count);
            Assert.AreEqual(1, game.Players[0].Pile.Count);

            InstantlyMakeTurn(game, Phase.Choose);
            InstantlyMakeTurn(game, Phase.Play);
            InstantlyMakeTurn(game, Phase.End);

            Assert.AreEqual(70, game.Players[1].Health);
            Assert.AreEqual(0, game.Players[0].Play.Count);
            Assert.AreEqual(3, game.Players[0].Pile.Count);
        }
        
        [Test]
        public void TestMastery()
        {
            var game = PlayNoCards(CardCollection.Mastery, CardCollection.Attack);
            Assert.AreEqual(11, game.Players[0].Deck.Count);
            Assert.AreEqual(1, game.Players[0].PutOff.Count);
            AssertIsOfType(CardCollection.Mastery, game.Players[0].PutOff[0]);

            for (int i = 0; i < rules.SuperDeckCount - 2; ++i)
            {
                game.Players[0].Pile.AddRange(game.Players[0].Deck);
                game.Players[0].Deck.Clear();

                InstantlyMakeTurn(game, Phase.Choose);
                InstantlyMakeTurn(game, Phase.Play);
                InstantlyMakeTurn(game, Phase.End);
                Assert.AreEqual(1, game.Players[0].PutOff.Count);
            }

            game.Players[0].Pile.AddRange(game.Players[0].Deck);
            game.Players[0].Deck.Clear();

            InstantlyMakeTurn(game, Phase.Choose);
            InstantlyMakeTurn(game, Phase.Play);
            InstantlyMakeTurn(game, Phase.End);
            Assert.AreEqual(0, game.Players[0].PutOff.Count);
        }

        [Test]
        public void TestEnchantedCards()
        {
            var game = PlayOneCard(CardCollection.EnchantedCards, CardCollection.Attack);
            Assert.AreEqual(70, game.Players[1].Health);
            Assert.AreEqual(0, game.Players[0].Hand.Count);
        }

        [Test]
        public void TestTakeAim()
        {
            var game = PlayOneCard(CardCollection.TakeAim, CardCollection.Attack);
            Assert.AreEqual(100, game.Players[1].Health);
            Assert.AreEqual(1, game.Players[0].Play.Count);

            InstantlyMakeTurn(game, Phase.Choose);
            InstantlyMakeTurn(game, Phase.Play);
            InstantlyMakeTurn(game, Phase.End);
            Assert.AreEqual(100, game.Players[1].Health);
            Assert.AreEqual(1, game.Players[0].Play.Count);

            InstantlyMakeTurn(game, Phase.Choose);
            game.Players[0].MakeATurn(game.Players[0].Hand.First());
            InstantlyMakeTurn(game, Phase.Play);
            InstantlyMakeTurn(game, Phase.End);
            Assert.AreEqual(70, game.Players[1].Health);
            Assert.AreEqual(0, game.Players[0].Play.Count);
        }

        [Test]
        public void TestRegeneration()
        {
            var game = PlayOneCard(CardCollection.Regeneration, CardCollection.Attack);
            Assert.AreEqual(100, game.Players[0].Health);
            Assert.AreEqual(1, game.Players[0].Play.Count);

            InstantlyMakeTurn(game, Phase.Choose);
            InstantlyMakeTurn(game, Phase.Play);
            InstantlyMakeTurn(game, Phase.End);
            Assert.AreEqual(100, game.Players[0].Health);
            Assert.AreEqual(1, game.Players[0].Play.Count);
            
            InstantlyMakeTurn(game, Phase.Choose);
            game.Players[0].Attack(75);
            InstantlyMakeTurn(game, Phase.Play);
            InstantlyMakeTurn(game, Phase.End);
            Assert.AreEqual(50, game.Players[0].Health);
            Assert.AreEqual(0, game.Players[0].Play.Count);
        }

        [Test]
        public void TestWeaponCrate()
        {
            var game = PlayOneCard(CardCollection.WeaponCrate, CardCollection.Attack);
            Assert.AreEqual(100, game.Players[1].Health);
            Assert.AreEqual(1, game.Players[0].Pile.Count);
            Assert.AreEqual(12, game.Players[0].Deck.Count);

            game = PlayOneCard(CardCollection.WeaponCrate, CardCollection.MightyStrike);
            Assert.AreEqual(95, game.Players[1].Health);
            Assert.AreEqual(2, game.Players[0].Pile.Count);
            Assert.AreEqual(11, game.Players[0].Deck.Count);
        }

        [Test]
        public void TestCombustion()
        {
            var game = PlayOneCard(CardCollection.Combustion, CardCollection.Attack);
            Assert.AreEqual(100, game.Players[1].Health);
            Assert.AreEqual(1, game.Players[0].Play.Count);
            Assert.AreEqual(3, game.Players[1].Hand.Count);
            Assert.AreEqual(0, game.Players[1].Pile.Count);
            
            InstantlyMakeTurn(game, Phase.Choose);
            game.Players[1].Play.Add(new CardState(CardCollection.Blessing, game.Players[1]));
            game.Players[1].Deck.Insert(0, new CardState(CardCollection.Block, game.Players[1]));
            InstantlyMakeTurn(game, Phase.Play);
            InstantlyMakeTurn(game, Phase.End);
            Assert.AreEqual(85, game.Players[1].Health);
            Assert.AreEqual(0, game.Players[0].Play.Count);
            Assert.AreEqual(4, game.Players[1].Hand.Count);
            Assert.AreEqual(2, game.Players[1].Pile.Count);
            AssertIsOfType(CardCollection.Block, game.Players[1].Pile[0]);
            AssertIsOfType(CardCollection.Blessing, game.Players[1].Pile[1]);
        }
        
        [Test]
        public void TestCopycat()
        {
            var game = InitializeState(CardCollection.Attack, CardCollection.Attack, CardCollection.Attack);
            game.Players[0].Draw();
            game.Players[0].Draw();
            game.Players[0].Play.Add(new CardState(CardCollection.Copycat, game.Players[0]));
            game.Players[0].Play.Add(new CardState(CardCollection.EnchantedCards, game.Players[0]));
            InstantlyMakeTurn(game, Phase.Play);
            InstantlyMakeTurn(game, Phase.End);

            Assert.AreEqual(10, game.Players[1].Health);
            Assert.AreEqual(1, game.Players[0].Hand.Count);
            Assert.AreEqual(6, game.Players[0].Pile.Count);
        }

        [Test]
        public void TestRobbery()
        {
            var game = PlayOneCard(CardCollection.Robbery, CardCollection.Attack);
            Assert.AreEqual(80, game.Players[1].Health);
            Assert.AreEqual(3, game.Players[1].Hand.Count);
            Assert.AreEqual(11, game.Players[1].Deck.Count);
            Assert.AreEqual(1, game.Players[1].Pile.Count);
        }

        [Test]
        public void TestHealingRain()
        {
            var game = InitializeState(CardCollection.HealingRain, CardCollection.HealingRain, CardCollection.HealingRain);
            game.Players[0].Attack(155);
            game.Players[0].MakeATurn(game.Players[0].Hand.First());
            InstantlyMakeTurn(game, Phase.Play);
            InstantlyMakeTurn(game, Phase.End);

            Assert.AreEqual(95, game.Players[0].Health);
            Assert.AreEqual(15, game.Players[0].Pile.Count);
        }

        [Test]
        public void TestBomb()
        {
            var game = PlayOneCard(CardCollection.Bomb, CardCollection.Attack);
            Assert.AreEqual(85, game.Players[1].Health);
            Assert.AreEqual(0, game.Players[0].Pile.Count);
            Assert.AreEqual(1, game.Players[0].PutOff.Count);
            AssertIsOfType(CardCollection.Bomb, game.Players[0].PutOff[0]);
        }

        [Test]
        public void TestEvolution()
        {
            var game = PlayOneCard(CardCollection.Evolution, CardCollection.Evolution);
            Assert.AreEqual(90, game.Players[1].Health);

            InstantlyMakeTurn(game, Phase.Choose);
            game.Players[0].MakeATurn(game.Players[0].Hand[0]);
            InstantlyMakeTurn(game, Phase.Play);
            InstantlyMakeTurn(game, Phase.End);
            Assert.AreEqual(80, game.Players[1].Health);

            InstantlyMakeTurn(game, Phase.Choose);
            game.Players[0].MoveCard(game.Players[0].Pile[0], Zone.Play);
            InstantlyMakeTurn(game, Phase.Play);
            InstantlyMakeTurn(game, Phase.End);
            Assert.AreEqual(65, game.Players[1].Health);
        }

        [Test]
        public void TestFireTank()
        {
            var game = PlayOneCard(CardCollection.FireTank, CardCollection.FireTank);
            Assert.AreEqual(90, game.Players[1].Health);
            Assert.AreEqual(0, game.Players[0].Pile.Count);
            Assert.AreEqual(1, game.Players[0].Play.Count);

            InstantlyMakeTurn(game, Phase.Choose);
            game.Players[0].MakeATurn(game.Players[0].Hand[0]);
            InstantlyMakeTurn(game, Phase.Play);
            InstantlyMakeTurn(game, Phase.End);
            Assert.AreEqual(80, game.Players[1].Health);
            Assert.AreEqual(0, game.Players[0].Pile.Count);
            Assert.AreEqual(2, game.Players[0].Play.Count);

            InstantlyMakeTurn(game, Phase.Choose);
            InstantlyMakeTurn(game, Phase.Play);
            InstantlyMakeTurn(game, Phase.End);
            Assert.AreEqual(50, game.Players[1].Health);
            Assert.AreEqual(2, game.Players[0].Pile.Count);
            Assert.AreEqual(0, game.Players[0].Play.Count);
        }

        [Test]
        public void TestSpikeTrap()
        {
            var game = PlayTwoCards(CardCollection.SpikeTrap, CardCollection.Attack, CardCollection.Attack);
            Assert.AreEqual(100, game.Players[1].Health);
            Assert.AreEqual(0, game.Players[0].Pile.Count);
            Assert.AreEqual(1, game.Players[0].Play.Count);

            InstantlyMakeTurn(game, Phase.Choose);
            game.Players[1].MakeATurn(game.Players[1].Hand[0]);
            InstantlyMakeTurn(game, Phase.Play);
            InstantlyMakeTurn(game, Phase.End);
            Assert.AreEqual(85, game.Players[1].Health);
            Assert.AreEqual(1, game.Players[0].Pile.Count);
            Assert.AreEqual(0, game.Players[0].Play.Count);
        }

        [Test]
        public void TestCircularSaw()
        {
            var game = PlayOneCard(CardCollection.CursedFlame, CardCollection.CircularSaw);
            Assert.AreEqual(65, game.Players[1].Health);
            Assert.AreEqual(1, game.Players[0].Hand.Count);
            Assert.AreEqual(2, game.Players[0].Pile.Count);
            Assert.AreEqual(0, game.Players[0].Play.Count);
            AssertIsOfType(CardCollection.CursedFlame, game.Players[0].Pile[0]);
            AssertIsOfType(CardCollection.CircularSaw, game.Players[0].Pile[1]);
        }

        [Test]
        public void TestFirstAid()
        {
            var game = PlayTwoCards(CardCollection.FirstAid, CardCollection.Attack, CardCollection.Heal);
            Assert.AreEqual(95, game.Players[0].Health);
            Assert.AreEqual(3, game.Players[0].Hand.Count);

            game = PlayTwoCards(CardCollection.FirstAid, CardCollection.Attack, CardCollection.Attack);
            Assert.AreEqual(95, game.Players[0].Health);
            Assert.AreEqual(2, game.Players[0].Hand.Count);
        }

        [Test]
        public void TestCombo()
        {
            var game = PlayOneCard(CardCollection.Combo, CardCollection.Attack);
            Assert.AreEqual(85, game.Players[1].Health);
            Assert.AreEqual(3, game.Players[0].Hand.Count);

            game = PlayOneCard(CardCollection.Combo, CardCollection.Heal);
            Assert.AreEqual(100, game.Players[1].Health);
            Assert.AreEqual(3, game.Players[0].Hand.Count);
        }
    }
}
