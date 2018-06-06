using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.Effects
{
    public abstract class RevealEffect : Effect
    {
        public CardState Revealed { get; protected set; }

        public RevealEffect(Action<EffectPerspective> call, bool disablable = true) :
            base(call, disablable)
        {}
    }
}
