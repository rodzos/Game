using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.Effects
{
    public class AttackEffect : Effect
    {
        public int Value { get; private set; }

        public AttackEffect(int value) :
            base(perspective => perspective.Player.Attack(value))
        {
            Value = value;
        }
    }
}
