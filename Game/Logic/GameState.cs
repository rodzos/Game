using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic
{
    public class GameState
    {
        private static ReadOnlyCollection<bool> buffOrder = new ReadOnlyCollection<bool>(new List<bool>{ true, false });

        public ReadOnlyCollection<PlayerState> Players { get; private set; }
        public Settings Settings { get; private set; }
        public Random Random { get; private set; }
        public int Turn { get; private set; }

        public GameState(List<List<CardType>> decks, Settings settings = null, Random random = null)
        {
            if (decks.Count != 2)
                throw new ArgumentException();
            Settings = settings ?? new Settings();
            Random = random ?? new Random();
            Players = new ReadOnlyCollection<PlayerState>(decks.Select(deck => new PlayerState(this, deck)).ToList());
            Turn = 1;
        }

        public PlayerState Opponent(PlayerState player)
        {
            var index = Players.IndexOf(player);
            if (index == -1)
                throw new ArgumentException();
            return Players[1 - index];
        }

        public void FinishTurn()
        {
            ++Turn;
        }

        public IEnumerable<EffectCall> MakeTurn(Phase phase, Func<EffectMaker> globalEffects) // TODO initializer not list
        {
            var globalEffectMakers = new List<EffectMaker> { globalEffects(), globalEffects() };

            var effects = new EffectSequence();

            bool changed = true;
            int count = 0;
            while (changed & count < 1000) // TODO
            {
                ++count;
                changed = false;
                foreach (bool buff in buffOrder)
                {
                    for (int i = 0; i < Players.Count; ++i)
                    {
                        bool changed2 = true;
                        while (changed2)
                        {
                            changed2 = false;
                            var player = Players[i];
                            var globalPerspective = new EffectMakerPerspective(phase, player, effects);
                            if (effects.AddFromEffectMaker(globalEffectMakers[i], globalPerspective, buff))
                            {
                                yield return effects.Effects.Last();
                                changed = true;
                                changed2 = true;
                                continue;
                            }
                            foreach (var card in Players[i].AllCards().ToList()) // TODO Order could be better
                            {
                                var perspective = new EffectMakerPerspective(phase, player, effects, card);
                                if (effects.AddFromEffectMaker(card.Effects, perspective, buff))
                                {
                                    yield return effects.Effects.Last();
                                    changed = true;
                                    changed2 = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
