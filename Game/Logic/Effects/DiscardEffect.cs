using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.Effects
{
    public class DiscardEffect : Effect
    {
        public CardState Discarded { get; protected set; }

        public DiscardEffect() :
            base(null)
        {
            base.Call = perspective =>
            {
                Discarded = perspective.Player.Discard();
            };
        }
    }
}
