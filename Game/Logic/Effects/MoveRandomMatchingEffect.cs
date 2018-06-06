using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.Effects
{
    public class MoveRandomMatchingEffect : MoveEffect
    {
        public Func<CardState, bool> Predicate { get; private set; }

        public MoveRandomMatchingEffect(Zone from, Zone to, Func<CardState, bool> predicate) :
            base(from, to)
        {
            Predicate = predicate;
            base.Call = perspective =>
            {
                if (perspective.Player.Cards[from].Count > 0)
                {
                    Moved = perspective.Random.ChoiceOrDefault(perspective.Player.Cards[from].Where(predicate));
                    if (Moved != null)
                        perspective.Player.MoveCardFrom(Moved, from, to);
                };
            };
        }
    }
}
