using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.Effects
{
    public class DrawMatchingEffect: DrawEffect
    {
        public Func<CardState, bool> Predicate { get; private set; }

        public DrawMatchingEffect(Func<CardState, bool> predicate)
        {
            base.Call = perspective => {
                Drawn = perspective.Player.DrawMatching(predicate);
                Success = Drawn != null && Drawn.Zone == Zone.Hand;
            };
            Predicate = predicate;
        }
    }
}
