using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic
{
    public abstract class EffectMaker
    {
        public abstract Effect GetEffect(EffectMakerPerspective perspective, bool buff);
    }
}
