using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.Effects
{
    public class HideEffect : Effect
    {
        public HideEffect() :
            base(perspective =>
            {
                if (perspective.Card != null)
                    perspective.Card.Hidden = true;
            })
        { }
    }
}
