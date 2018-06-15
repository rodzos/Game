using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Game.Logic;
using Game.Logic.EffectMakers;
using Game.Logic.Effects;
using Game.Logic.Interactor;
using static Game.Logic.CardCollection;

namespace Game
{
    static class Program
    {
        public static void Main(string[] args)
        {
            var rules = new GameRules();
            var random = new Random();
            var decks = new List<Deck>();
            for (int i = 0; i < 2; ++i)
                decks.Add(Deck.MakeRandomDeck(AllCards, rules, random));

            var interactors = new List<IInteractor>();
            interactors.Add(new Logic.Interactor.Interactors.ConsoleInteractor());
            interactors.Add(new Logic.Interactor.Interactors.AIInteractor());

            var game = new Logic.Interactor.Game(decks, interactors);
            game.Play();
        }
    }
}
