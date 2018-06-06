using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic
{
    public class EffectCall
    {
        public Effect Effect { get; private set; }
        public PlayerState Caller { get; private set; }
        public CardState CallerCard { get; private set; }
        public PlayerState Target { get; private set; }
        public bool Buff { get; private set; }
        public bool Disabled => Effect.Disabled;

        public EffectCall(Effect effect, PlayerState caller, CardState callerCard, PlayerState target)
        {
            Effect = effect;
            Caller = caller;
            CallerCard = callerCard;
            if (CallerCard != null && CallerCard.Owner != Caller)
                throw new ArgumentException();
            Target = target;
            Buff = caller == target;
        }

        public EffectCall(Effect effect, PlayerState caller, CardState callerCard, bool buff)
        {
            Effect = effect;
            Caller = caller;
            CallerCard = callerCard;
            if (CallerCard != null && CallerCard.Owner != Caller)
                throw new ArgumentException();
            Target = buff ? caller : caller.Game.Opponent(caller);
            Buff = buff;
        }

        public void Call()
        {
            Effect.Call(new EffectPerspective(Target, Buff ? CallerCard : null));
        }
    }
}
