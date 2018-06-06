using Game.Logic.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.EffectMakers.CardEffectMakers
{
    public class EvolutionEffectMaker : EffectMaker
    {
        private int state = 0;
        private int damage = 10;

        public EvolutionEffectMaker()
        { }

        public override Effect GetEffect(EffectMakerPerspective perspective, bool buff)
        {
            if (perspective.Phase == Phase.StartOfGame && perspective.Card.Zone == Zone.PutOff)
                return new StartInDeckEffect();
            if (state == 0 && perspective.IsPlayed() && !buff)
            {
                ++state;
                damage += 5;
                return new AttackEffect(damage - 5);
            }
            if (state == 1 && perspective.IsEnded())
            {
                state = 0;
                return new EndEffect();
            }
            return null;
        }
    }
}
