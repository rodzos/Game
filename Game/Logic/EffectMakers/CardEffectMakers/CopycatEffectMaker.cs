using Game.Logic.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.EffectMakers.CardEffectMakers
{
    public class CopycatEffectMaker : EffectMaker
    {
        private List<CardState> clonedCards = new List<CardState>();
        private List<Effect> clonedEffects = new List<Effect>();

        public CopycatEffectMaker()
        { }

        public override Effect GetEffect(EffectMakerPerspective perspective, bool buff)
        {
            if (perspective.Phase == Phase.StartOfGame && perspective.Card.Zone == Zone.PutOff)
                return new StartInDeckEffect();
            if (!perspective.IsPlayed())
            {
                clonedCards.Clear();
                clonedEffects.Clear();
                if (perspective.Card.Zone == Zone.Play)
                    return new EndEffect();
                return null;
            }
            while (clonedCards.Count < 3)
            {
                var card = perspective.Player.Play.FirstOrDefault(x => !clonedCards.Contains(x) && x != perspective.Card);
                if (card == null)
                    break;
                clonedCards.Add(card);
            }
            var clone = perspective.PrevEffects.FirstOrDefault(x => x.Caller == perspective.Player && x.Buff == buff &&
                    clonedCards.Contains(x.CallerCard) && !clonedEffects.Contains(x.Effect));
            if (clone != null)
            {
                clonedEffects.Add(clone.Effect);
                return clone.Effect;
            }
            return null;
        }
    }
}
