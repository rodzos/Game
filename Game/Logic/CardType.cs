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
        public String Description { get; private set; }
        public Rarity Rarity { get; private set; }
        public Func<EffectMaker> Effects { get; private set; }
        public ReadOnlyCollection<Tag> Tags => new ReadOnlyCollection<Tag>(tags);

        private readonly List<Tag> tags;

        public CardType(String name, String description, Rarity rarity, Func<EffectMaker> effects, List<Tag> tags)
        {
            Name = name;
            Description = description;
            Rarity = rarity;
            Effects = effects;
            this.tags = tags;
        }

        public CardType(String name, String description, Rarity rarity, Func<EffectMaker> effects, params Tag[] tags) :
            this(name, description, rarity, effects, tags.ToList())
        { }

        public IEnumerable<EffectCall> CallIsolated(GameState model, Random random = null)
        {
            var decks = new List<Deck> { new Deck(model.GameRules), new Deck(model.GameRules) };
            decks[0].Add(this);
            var game = new GameState(decks, model.GlobalEffects, model.GameRules, random);
            game.Players[0].MoveCard(game.Players[0].PutOff[0], Zone.Hand);
            game.Players[0].MakeATurn(game.Players[0].Hand[0]);
            foreach (var e in game.MakeTurn(Phase.Play))
                yield return e;
        }
    }
}
