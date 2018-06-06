using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic
{
    public class EffectPerspective
    {
        public PlayerState Player { get; private set; }
        public CardState Card { get; private set; }

        public Settings Settings => Player.Settings;
        public Random Random => Player.Random;

        public EffectPerspective(PlayerState player, CardState card)
        {
            Player = player;
            Card = card;
        }
    }
}
