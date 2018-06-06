using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.Effects
{
    public class MoveAllMatchingEffect : MoveAllEffect
    {
        public Func<CardState, bool> Predicate { get; private set; }

        public MoveAllMatchingEffect(Zone from, Zone to, Func<CardState, bool> predicate) :
            base(from, to)
        {
            Predicate = predicate;
            base.Call = perspective =>
            {
                Moved = perspective.Player.Cards[from].Where(predicate).ToList();
                foreach (var card in Moved)
                    perspective.Player.MoveCardFrom(card, from, to);
            };
        }
    }
}
