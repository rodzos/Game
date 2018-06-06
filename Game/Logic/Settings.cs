using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic
{
    public class Settings
    {
        public int HandMax { get; private set; } = 7;
        public int StartingHand { get; private set; } = 3;
        public bool Mulligan { get; private set; } = false;
        public bool StrictHandMax { get; private set; } = true;
        // TODO discard on overdraw
        public bool FailedDrawingFatigues { get; private set; } = true;
        public int FatigueDeckCount { get; private set; } = 5;
        public int FatigueValue { get; private set; } = 5;
        public int SuperDeckCount { get; private set; } = 3;
        public int MaxHealth { get; private set; } = 100;
        public bool CapHealthBelowDuringTurn { get; private set; } = false;
        public bool CapHealthAboveDuringTurn { get; private set; } = true;
    }
}
