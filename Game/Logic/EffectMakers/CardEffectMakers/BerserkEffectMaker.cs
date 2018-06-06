using Game.Logic.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.EffectMakers.CardEffectMakers
{
    class BerserkEffectMaker : EffectMaker
    {
        private int state = 0;
        private int damage = 0;
        private int healing = 0;

        public BerserkEffectMaker()
        { }

        public override Effect GetEffect(EffectMakerPerspective perspective, bool buff)
        {
            if (perspective.Phase == Phase.StartOfGame && perspective.Card.Zone == Zone.PutOff)
                return new StartInDeckEffect();
            if (state == 0 && perspective.IsPlayed() && !buff)
            {
                state = 1;
                return new AttackEffect(5);
            }
            if (state == 1)
            {
                if (perspective.IsEnded() && buff)
                {
                    state = 0;
                    damage = 0;
                    healing = 0;
                    return new EndEffect();
                }
                if (perspective.IsPlayed())
                {
                    if (!buff)
                        damage = perspective.PrevEffects.CountAttackTaken(perspective.Opponent);
                    else if (damage > healing)
                    {
                        var value = damage - healing;
                        healing = damage;
                        return new HealEffect(value);
                    }
                }
            }
            return null;
        }
    }
}
