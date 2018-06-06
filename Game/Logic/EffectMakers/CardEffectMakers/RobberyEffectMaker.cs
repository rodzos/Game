using Game.Logic.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.EffectMakers.CardEffectMakers
{
    public class RobberyEffectMaker : EffectMaker
    {
        private int state = 0;
        private MoveEffect moveEffect = null;
        private EffectMaker stolenMaker = null;

        public RobberyEffectMaker()
        { }

        public override Effect GetEffect(EffectMakerPerspective perspective, bool buff)
        {
            if (perspective.Phase == Phase.StartOfGame && perspective.Card.Zone == Zone.PutOff)
                return new StartInDeckEffect();
            if (state == 0 && perspective.IsPlayed() && !buff)
            {
                ++state;
                return new AttackEffect(5);
            }
            if (state == 1 && perspective.IsPlayed() && !buff)
            {
                ++state;
                moveEffect = new MoveRandomEffect(Zone.Deck, Zone.Pile);
                return moveEffect;
            }
            if (state == 2)
            {
                if (stolenMaker == null && moveEffect.Moved != null)
                    stolenMaker = moveEffect.Moved.Type.Effects();
                if (perspective.IsEnded())
                {
                    state = 0;
                    moveEffect = null;
                    stolenMaker = null;
                    return new EndEffect();
                }
                if (perspective.IsPlayed() && stolenMaker != null)
                    return stolenMaker.GetEffect(perspective, buff);
            }
            return null;
        }
    }
}
