using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Game.Logic.EffectMakers
{
    public class SequentialEffectMaker : EffectMaker
    {
        private List<SequentialEffectMakerItem> items;
        private int pos;

        public ReadOnlyCollection<SequentialEffectMakerItem> Items {
            get { return new ReadOnlyCollection<SequentialEffectMakerItem>(items); }
        }

        public SequentialEffectMaker(List<SequentialEffectMakerItem> items)
        {
            this.items = items;
        }

        public SequentialEffectMaker(params SequentialEffectMakerItem[] items)
        {
            this.items = new List<SequentialEffectMakerItem>(items);
        }

        public override Effect GetEffect(EffectMakerPerspective perspective, bool buff)
        {
            while (items[pos].SkipCondition(perspective)) // TODO infinite loop
            {
                pos = (pos + 1) % items.Count;
            }
            if (items[pos].Buff == buff && items[pos].PlayCondition(perspective))
            {
                var result = items[pos].Effect(perspective);
                pos = (pos + 1) % items.Count;
                return result;
            }
            return null;
        }
    }
}
