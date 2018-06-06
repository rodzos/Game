using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.Effects
{
    public class RevealRandomEffect : RevealEffect
    {
        public Zone Zone { get; private set; }

        public RevealRandomEffect(Zone zone) :
            base(null)
        {
            Zone = zone;
            base.Call = perspective =>
            {
                if (perspective.Player.Cards[Zone].Count == 0)
                    Revealed = null;
                else
                    Revealed = perspective.Random.Choice(perspective.Player.Cards[Zone]);
            };
        }
    }
}
