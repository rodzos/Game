using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.Effects
{
    public class BlockEffect : Effect
    {
        public int Value { get; private set; }

        public BlockEffect(int value) :
            base(perspective => perspective.Player.Block(value))
        {
            Value = value;
        }
    }
}
