using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.Effects
{
    public class DrawEffect : Effect
    {
        public CardState Drawn { get; protected set; }
        public bool Success { get; protected set; }

        public DrawEffect() :
            base(null)
        {
            base.Call = perspective =>
            {
                Drawn = perspective.Player.Draw();
                Success = Drawn != null && Drawn.Zone == Zone.Hand;
            };
        }
    }
}
