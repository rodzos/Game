using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Game.Logic;
using Game.Logic.EffectMakers;
using Game.Logic.Interactor;
using Ninject;
using Game.Logic.CardCollection;
using System.IO;

namespace Game
{
    static class Program
    {
        public static void Main(string[] args)
        {
            var container = new StandardKernel();

            container.Bind<TextReader>().ToConstant(Console.In);
            container.Bind<TextWriter>().ToConstant(Console.Out);

            container.Bind<Func<EffectMaker>>().ToConstant<Func<EffectMaker>>(() => new GlobalEffects());
            container.Bind<GameRules>().ToSelf();
            container.Bind<Random>().ToSelf();
            container.Bind<ICardCollection>().To<CardCollection>();

            container.Bind<Deck>().ToMethod(x => Deck.MakeRandomDeck(
                x.Kernel.Get<CardCollection>(), x.Kernel.Get<GameRules>(), x.Kernel.Get<Random>()));
            container.Bind<Deck>().ToMethod(x => x.Kernel.GetAll<Deck>().First());

            container.Bind<IInteractor>().To<Logic.Interactor.Interactors.ConsoleInteractor>();
            container.Bind<IInteractor>().To<Logic.Interactor.Interactors.AIInteractor>();

            container.Bind<Logic.Interactor.Game>().ToSelf()
                .OnActivation(x => x.Play());

            var game = container.Get<Logic.Interactor.Game>();
        }
    }
}
