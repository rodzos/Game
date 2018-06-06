using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.Effects
{
    public class MoveEffect : Effect
    {
        public Zone From { get; protected set; }
        public Zone To { get; protected set; }
        public CardState Moved { get; protected set; }

        public MoveEffect(Zone from, Zone to) :
            base(null)
        {
            From = from;
            To = to;
            base.Call = perspective =>
            {
                if (perspective.Card.Zone == from)
                {
                    Moved = perspective.Card;
                    perspective.Player.MoveCard(Moved, to);
                }
                else
                    Moved = null;
            };
        }
    }
}
