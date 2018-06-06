using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.Effects
{
    public class AutoEffect : Effect
    {
        public AutoEffect() :
            base(perspective =>
            {
                if (perspective.Card != null)
                    perspective.Player.MoveCard(perspective.Card, Zone.Play);
            })
        { }
    }
}
