using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic
{
    public class EffectMakerPerspective
    {
        // TODO I have a plan to merge this with EffectPerspective and make it unable to change the game state
        public Phase Phase { get; private set; }
        public GameState Game { get; private set; }
        private int playerIndex;
        public EffectSequence PrevEffects { get; private set; }

        public PlayerState Player => Game.Players[playerIndex];
        public PlayerState Opponent => Game.Players[1 - playerIndex];
        public Settings Settings => Game.Settings;
        public Random Random => Game.Random;

        public CardState Card { get; private set; }

        public EffectMakerPerspective(Phase phase, PlayerState player, EffectSequence prefEffects, CardState card = null)
        {
            Phase = phase;
            Game = player.Game;
            playerIndex = Game.Players.IndexOf(player);
            if (playerIndex == -1)
                throw new ArgumentException();
            PrevEffects = prefEffects;
            if (card != null && card.Owner != player)
                throw new ArgumentException();
            Card = card;
        }

        public bool IsPlayed()
        {
            return Phase == Phase.Play && (Card == null || Card.Zone == Zone.Play);
        }

        public bool IsEnded()
        {
            return Phase == Phase.End && (Card == null || Card.Zone == Zone.Play);
        }

        public int CountAttackTaken()
        {
            return PrevEffects.CountAttackTaken(Player);
        }
    }
}
