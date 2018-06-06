using Game.Logic.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.EffectMakers.CardEffectMakers
{
    class MindReadingEffectMaker : EffectMaker
    {
        private int state = 0;
        private RevealEffect revealEffect = null;
        private EffectMaker stolenMaker = null;

        public MindReadingEffectMaker()
        {}

        public override Effect GetEffect(EffectMakerPerspective perspective, bool buff)
        {
            if (perspective.Phase == Phase.StartOfGame && perspective.Card.Zone == Zone.PutOff)
                return new StartInDeckEffect();
            if (state == 0 && perspective.IsPlayed() && buff)
            {
                ++state;
                return new HealEffect(5);
            }
            if (state == 1 && perspective.IsPlayed() && !buff)
            {
                ++state;
                return new AttackEffect(5);
            }
            if (state == 2 && perspective.IsPlayed() && !buff)
            {
                ++state;
                revealEffect = new RevealRandomEffect(Zone.Hand);
                return revealEffect;
            }
            if (state == 3)
            {
                if (stolenMaker == null && revealEffect.Revealed != null)
                    stolenMaker = revealEffect.Revealed.Type.Effects();
                if (perspective.IsEnded())
                {
                    state = 0;
                    revealEffect = null;
                    stolenMaker = null;
                    return new EndEffect();
                }
                if (perspective.IsPlayed() && stolenMaker != null)
                {
                    return stolenMaker.GetEffect(perspective, buff);
                }
            }
            return null;
        }
    }
}
