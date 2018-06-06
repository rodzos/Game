using Game.Logic.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.EffectMakers.CardEffectMakers
{
    public class CloneEffectMaker : EffectMaker
    {
        private int state = 0;
        private CardState saved = null;

        public CloneEffectMaker()
        { }

        public override Effect GetEffect(EffectMakerPerspective perspective, bool buff)
        {
            if (perspective.Phase == Phase.StartOfGame && perspective.Card.Zone == Zone.PutOff)
                return new StartInDeckEffect();
            if (perspective.IsEnded())
            {
                state = 0;
                saved = null;
                return new EndEffect();
            }
            if (buff && perspective.Card.Zone == Zone.Hand)
            {
                if (saved == null)
                    saved = perspective.PrevEffects
                        .Where(x => x.Target == perspective.Player && x.Effect is DiscardEffect)
                        .Select(x => ((DiscardEffect)x.Effect).Discarded)
                        .FirstOrDefault(x => x != null && (x.Zone == Zone.Pile || x.Zone == Zone.Deck));
                if (saved != null)
                {
                    if (state == 0)
                    {
                        ++state;
                        return new MoveMatchingEffect(saved.Zone, Zone.Hand, x => x == saved);
                    }
                    if (state == 1)
                    {
                        ++state;
                        return new MoveEffect(perspective.Card.Zone, Zone.Play);
                    }
                }
            }
            if (buff && perspective.IsPlayed())
            {
                if (state < 2)
                    state = 2;
                if (state == 2)
                {
                    ++state;
                    return new BlockEffect(10);
                }
                if (state == 3)
                {
                    ++state;
                    return new DrawEffect();
                }
            }
            return null;
        }
    }
}
