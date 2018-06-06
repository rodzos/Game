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
        private int cardsLeft;

        public GlobalEffects()
        {
            done = false;
            cardsLeft = 0;
        }

        public override Effect GetEffect(EffectMakerPerspective perspective, bool buff)
        {
            if (cardsLeft > 0)
            {
                --cardsLeft;
                return new DrawEffect();
            }
            if (done || perspective.IsPlayed() || !buff)
                return null;
            done = true;
            if (perspective.IsEnded())
            {
                return new FinishTurnEffect();
            }
            else if (perspective.Phase == Phase.Choose)
            {
                if (perspective.Game.Turn == 1)
                {
                    cardsLeft = perspective.Settings.StartingHand - 1;
                    return new DrawEffect();
                }
                else if (perspective.Settings.StrictHandMax || perspective.Player.Hand.Count < perspective.Settings.HandMax)
                    return new DrawEffect();
            }
            return null;
        }
    }
}
