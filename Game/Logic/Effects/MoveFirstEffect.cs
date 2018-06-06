using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.Effects
{
    public class MoveFirstEffect: MoveEffect
    {
        public MoveFirstEffect(Zone from, Zone to):
            base(from, to)
        {
            From = from;
            To = to;
            base.Call = perspective =>
            {
                Moved = perspective.Player.Cards[from].FirstOrDefault();
                if (Moved != null)
                    perspective.Player.MoveCard(Moved, to);
            };
        }
    }
}
