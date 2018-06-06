using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.Effects
{
    public class RevealMatchingEffect : RevealEffect
    {
        public Zone Zone { get; private set; }
        Func<CardState, bool> Predicate;

        public RevealMatchingEffect(Zone zone, Func<CardState, bool> predicate) :
            base(null)
        {
            Zone = zone;
            Predicate = predicate;
            base.Call = perspective =>
            {
                Revealed = perspective.Player.Cards[Zone].FirstOrDefault(predicate);
            };
        }
    }
}
