using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.Effects
{
    public class DiscardMatchingEffect : DiscardEffect
    {
        public Func<CardState, bool> Predicate { get; private set; }

        public DiscardMatchingEffect(Func<CardState, bool> predicate)
        {
            base.Call = perspective => {
                Discarded = perspective.Player.DiscardMatching(predicate);
            };
            Predicate = predicate;
        }
    }
}
