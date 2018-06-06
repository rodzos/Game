using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic
{
    public class CardType
    {
        public String Name { get; private set; }
        public Rarity Rarity { get; private set; }
        public Func<EffectMaker> Effects { get; private set; }
        public ReadOnlyCollection<Tag> Tags => new ReadOnlyCollection<Tag>(tags);

        private List<Tag> tags;

        public CardType(String name, Rarity rarity, Func<EffectMaker> effects, List<Tag> tags)
        {
            Name = name;
            Rarity = rarity;
            Effects = effects;
            this.tags = tags;
        }

        public CardType(String name, Rarity rarity, Func<EffectMaker> effects, params Tag[] tags)
        {
            Name = name;
            Rarity = rarity;
            Effects = effects;
            this.tags = tags.ToList();
        }

        public IEnumerable<EffectCall> CallIsolated(Settings settings = null, Random random = null)
        {
            var game = new GameState(new List<List<CardType>> { new List<CardType> { this }, new List<CardType>() }, settings, random);
            game.Players[0].MoveCard(game.Players[0].PutOff[0], Zone.Hand);
            game.Players[0].MakeATurn(game.Players[0].Hand[0]);
            foreach (var e in game.MakeTurn(Phase.Play, () => null))
                yield return e;
        }
    }
}
