using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.Effects
{
    public class FinishTurnEffect : Effect
    {
        public FinishTurnEffect() :
            base(perspective => { perspective.Player.FinishTurn(); })
        { }
    }
}
