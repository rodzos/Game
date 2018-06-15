using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.CardCollection
{
    public interface ICardCollection
    {
        IEnumerable<CardType> GetCards();
    }
}
