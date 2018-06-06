using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.Effects
{
    public class HealEffect : Effect
    {
        public int Value { get; private set; }

        public HealEffect(int value) :
            base(perspective => perspective.Player.Heal(value))
        {
            Value = value;
        }
    }
}
