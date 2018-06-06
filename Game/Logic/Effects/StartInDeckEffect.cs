using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.Effects
{
    public class StartInDeckEffect : Effect
    {
        public StartInDeckEffect() :
            base(perspective => perspective.Player.MoveCardFrom(perspective.Card, Zone.PutOff, Zone.Deck))
        { }
    }
}
