using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.Effects
{
    public class LoseMaxHealthEffect : Effect
    {
        public int Value { get; private set; }
        public LoseMaxHealthEffect(int value) :
            base(perspective => perspective.Player.LoseMaxHealth(value))
        {
            Value = value;
        }
    }
}
