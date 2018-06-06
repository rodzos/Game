using Game.Logic.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.EffectMakers
{
    public class GlobalEffects : EffectMaker
    {
        private bool done;

        public GlobalEffects()
        {
            done = false;
        }

        public override Effect GetEffect(EffectMakerPerspective perspective, bool buff)
        {
            if (done || perspective.Phase == Phase.Play || !buff)
                return null;
            done = true;
            if (perspective.Game.Turn == 1)
                return new DrawEffect(perspective.Settings.StartingHand);
            else if (perspective.Settings.StrictHandMax || perspective.Player.Hand.Count < perspective.Settings.HandMax)
                return new DrawEffect(1);
            return null;
        }
    }
}
