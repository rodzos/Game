using Game.Logic.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.EffectMakers.CardEffectMakers
{
    public class CurseOfFearEffectMaker : EffectMaker
    {
        private int state = 0;
        private int discarded = 0;

        public CurseOfFearEffectMaker() { }

        public override Effect GetEffect(EffectMakerPerspective perspective, bool buff)
        {
            if (perspective.Phase == Phase.StartOfGame && perspective.Card.Zone == Zone.PutOff)
                return new StartInDeckEffect();
            if (state == 0 && perspective.IsPlayed() && !buff)
            {
                ++state;
                return new AttackEffect(10);
            }
            if (state == 1 && perspective.IsPlayed() && !buff &&
                perspective.PrevEffects.CountAttackTaken(perspective.Opponent) - discarded * 15 > 15)
            {
                ++discarded;
                return new DiscardEffect();
            }
            if (perspective.IsEnded())
            {
                state = 0;
                discarded = 0;
                return new EndEffect();
            }
            return null;
        }
    }
}
