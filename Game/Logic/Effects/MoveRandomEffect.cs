using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.Effects
{
    public class MoveRandomEffect : MoveEffect
    {
        public MoveRandomEffect(Zone from, Zone to):
            base(from, to)
        {
            base.Call = perspective =>
            {
                if (perspective.Player.Cards[from].Count > 0)
                {
                    Moved = perspective.Random.Choice(perspective.Player.Cards[from]);
                    perspective.Player.MoveCardFrom(Moved, from, to);
                };
            };
        }
    }
}
