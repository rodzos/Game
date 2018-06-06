using Game.Logic.EffectMakers;
using Game.Logic.EffectMakers.CardEffectMakers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic
{
    public class CardState
    {
        public CardType Type { get; private set; }
        public PlayerState Owner { get; private set; }
        public EffectMaker Effects { get; private set; }
        public bool Hidden { get; set; }

        public String Name => Type.Name;
        public Zone Zone => Owner.FindCard(this);
        public Rarity Rarity => Type.Rarity;
        public ReadOnlyCollection<Tag> Tags => Type.Tags;

        public CardState(CardType type, PlayerState owner)
        {
            Type = type;
            Owner = owner;
            Effects = type.Effects();
            Hidden = false;
        }
    }
}
