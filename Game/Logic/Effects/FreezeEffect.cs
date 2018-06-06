using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.Effects
{
    public class FreezeEffect : MoveEffect
    {
        public FreezeEffect():
            base(Zone.Hand, Zone.Deck)
        {
            base.Call = perspective =>
            {
                Moved = perspective.Player.Cards[From].FirstOrDefault();
                if (Moved != null)
                {
                    perspective.Player.MoveCard(Moved, To);
                    if (perspective.Player.Deck.Contains(Moved))
                    {
                        perspective.Player.Deck.Remove(Moved);
                        perspective.Player.Deck.Insert(0, Moved);
                    }
                }
            };
        }
    }
}
