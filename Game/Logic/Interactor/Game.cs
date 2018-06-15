using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.Interactor
{
    public class Game
    {
        public delegate void ChioceEventHandler(object sender, CardState card);

        public GameState GameState { get; private set; }
        private List<IInteractor> interactors;
        private List<bool> ready = new List<bool> { false, false };
        private List<CardState> chosen = new List<CardState> { null, null };

        public bool Started { get; private set; } = false;

        public Game(List<Deck> decks, List<IInteractor> interactors,
            Func<EffectMaker> globalEffects = null, GameRules rules = null, Random random = null)
        {
            if (decks.Count != 2)
                throw new ArgumentException("Should have exactly 2 decks");
            if (interactors.Count != 2)
                throw new ArgumentException("Should have exactly 2 interactors");
            GameState = new GameState(decks, globalEffects ?? (() => new EffectMakers.GlobalEffects()),
                rules ?? new GameRules(), random ?? new Random());
            this.interactors = interactors.ToList();
            for (int i = 0; i < 2; ++i)
            {
                var player = GameState.Players[i];
                this.interactors[i].MakeChoice += (sender, card) => OnChosen(sender, player, card);
            }
        }

        public void Play()
        {
            if (Started)
                throw new InvalidOperationException("Game has already started");
            Started = true;
            for (int i = 0; i < 2; ++i)
                interactors[i].StartGame(this, GameState.Players[i]);
            StartFirstTurn();
            foreach (var e in this.interactors)
                e.AskForChoice();
        }

        private void MakeOneTurnPhase(Phase phase)
        {
            foreach (var effect in GameState.MakeTurn(phase))
            {
                effect.Call();
                foreach (var e in interactors)
                    e.DisplayEffect(effect);
            }
        }

        private void StartFirstTurn()
        {
            MakeOneTurnPhase(Phase.StartOfGame);
            MakeOneTurnPhase(Phase.Choose);
        }

        private void MakeTurn()
        {
            for (int i = 0; i < 2; ++i)
                if (chosen[i] != null)
                    GameState.Players[i].MakeATurn(chosen[i]);
            MakeOneTurnPhase(Phase.Play);
            MakeOneTurnPhase(Phase.End);
            MakeOneTurnPhase(Phase.Choose);
            if (GameState.IsEnded)
            {
                foreach (var e in interactors)
                    e.EndGame();
            }
            else
            {
                for (int i = 0; i < 2; ++i)
                    ready[i] = false;
                foreach (var e in interactors)
                    e.AskForChoice();
            }
        }

        private void OnChosen(object sender, PlayerState player, CardState card)
        {
            var playerIndex = GameState.Players.IndexOf(player);
            ready[playerIndex] = true;
            chosen[playerIndex] = card;
            if (ready.All(x => x))
                MakeTurn();
        }
    }
}
