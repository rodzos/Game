using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic
{
    public class GameRules
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
        public int MaxIterationPerTurn { get; private set; } = 100;
        public int MinDeckSize { get; private set; } = 15;
        public int MaxDeckSize { get; private set; } = 20;
        public ReadOnlyDictionary<Rarity, int> MaxRarityInDeck { get; private set; } = new ReadOnlyDictionary<Rarity, int>(
            new Dictionary<Rarity, int> { { Rarity.Common, 2 }, { Rarity.Rare, 1 } });
        public ReadOnlyDictionary<Tag, int> MaxTagInDeck { get; private set; } = new ReadOnlyDictionary<Tag, int>(
            new Dictionary<Tag, int> { { Tag.Auto, 2 }, { Tag.Super, 1 } });
    }
}
