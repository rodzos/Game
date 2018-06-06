using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.Effects
{
    public class MoveAllEffect : Effect
    {
        public Zone From { get; protected set; }
        public Zone To { get; protected set; }
        public ReadOnlyCollection<CardState> AllMoved => new ReadOnlyCollection<CardState>(Moved);

        public List<CardState> Moved { get; protected set; }

        public MoveAllEffect(Zone from, Zone to) :
            base(null)
        {
            From = from;
            To = to;
            base.Call = perspective =>
            {
                Moved = perspective.Player.Cards[from].ToList();
                foreach (var card in Moved)
                    perspective.Player.MoveCardFrom(card, from, to);
            };
        }
    }
}
