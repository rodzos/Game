using Game.Logic.Effects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic
{
    public class EffectSequence: IEnumerable<EffectCall>
    {
        public ReadOnlyCollection<EffectCall> Effects => new ReadOnlyCollection<EffectCall>(effects);
        private List<EffectCall> effects;

        public EffectSequence()
        {
            effects = new List<EffectCall>();
        }

        public IEnumerator<EffectCall> GetEnumerator()
        {
            return Effects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Effects.GetEnumerator();
        }

        public bool AddFromEffectMaker(EffectMaker effectMaker, EffectMakerPerspective perspective, bool buff)
        {
            if (effectMaker == null)
                return false;
            var effect = effectMaker.GetEffect(perspective, buff);
            if (effect != null)
            {
                var effectCall = new EffectCall(effect, perspective.Player, perspective.Card, buff);
                effects.Add(effectCall);
                return true;
            }
            return false;
        }

        public int CountAttackTaken(PlayerState player)
        {
            return Effects
                .Where(x => x.Target == player && x.Effect is AttackEffect)
                .Select(x => ((AttackEffect)x.Effect).Value)
                .Sum();
        }

        public List<EffectCall> EffectsFromCard(CardState card)
        {
            return Effects.Where(x => x.CallerCard == card).ToList();
        }
    }
}
