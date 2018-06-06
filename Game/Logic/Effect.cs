using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic
{
    public class Effect
    {
        public Action<EffectPerspective> Call { get; protected set; }

        public bool Disablable { get; private set; }
        public bool Disabled { get; private set; }

        public Effect(Action<EffectPerspective> call, bool disablable = true)
        {
            Call = call;
            Disablable = disablable;
        }
        
        public void Disable()
        {
            if (Disablable)
                Disabled = true;
        }
    }
}
