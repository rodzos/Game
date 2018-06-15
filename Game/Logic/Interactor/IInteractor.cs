using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.Interactor
{
    public interface IInteractor
    {
        void StartGame(Game game, PlayerState player);
        void EndGame();
        void AskForChoice();
        event Game.ChioceEventHandler MakeChoice;
        void DisplayEffect(EffectCall effect);
    }
}
