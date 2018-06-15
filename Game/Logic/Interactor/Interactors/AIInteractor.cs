using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.Interactor.Interactors
{
    class AIInteractor : IInteractor
    {
        private Game game;
        private PlayerState player;
        private Random random;

        public AIInteractor(Random random = null)
        {
            this.random = random ?? new Random();
        }

        public void StartGame(Game game, PlayerState player)
        {
            this.game = game;
            this.player = player;
        }

        public void EndGame()
        {
            game = null;
            player = null;
        }

        public void AskForChoice()
        {
            MakeChoice(this, random.ChoiceOrDefault(player.Hand));
        }

        public event Game.ChioceEventHandler MakeChoice = new Game.ChioceEventHandler((x, y) => { });


        public void DisplayEffect(EffectCall effect)
        {
            
        }
    }
}
