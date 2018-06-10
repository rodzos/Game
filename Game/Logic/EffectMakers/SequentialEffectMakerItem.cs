using Game.Logic.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.EffectMakers
{
    public class SequentialEffectMakerItem // TODO would be nice to implement Fluent API
    {
        public static SequentialEffectMakerItem StartInDeck()
        {
            return new SequentialEffectMakerItem(Phase.StartOfGame, Zone.PutOff, true,
                x => x.Game.Turn == 1, x => x.Game.Turn != 1 || x.Phase != Phase.StartOfGame, x => new StartInDeckEffect());
        }

        public static SequentialEffectMakerItem End()
        {
            return new SequentialEffectMakerItem(Phase.End, Zone.Play, true, x => new EndEffect());
        }
        
        public static SequentialEffectMakerItem Auto()
        {
            return new SequentialEffectMakerItem(Phase.Choose, Zone.Hand, true, x => true, x => x.Card.Zone == Zone.Play, x => new AutoEffect());
        }

        public static SequentialEffectMakerItem NextTurn()
        {
            return new SequentialEffectMakerItem(Phase.End, Zone.Play, false, x => null);
        }

        public static SequentialEffectMakerItem Super()
        {
            return new SequentialEffectMakerItem(Phase.End, Zone.PutOff, true,
                x => x.Player.DeckCount >= x.GameRules.SuperDeckCount,
                x => x.Card.Zone != Zone.PutOff,
                x => new StartInDeckEffect());
        }

        public static SequentialEffectMakerItem Hidden()
        {
            return new SequentialEffectMakerItem(true, x => x.Card.Zone != Zone.Play, x => false, x => new HideEffect());
        }

        public static SequentialEffectMakerItem PlayEffect(bool buff, Func<EffectMakerPerspective, Effect> effect)
        {
            return new SequentialEffectMakerItem(Phase.Play, Zone.Play, buff, effect);
        }

        public static SequentialEffectMakerItem ConditionalPlayEffect(bool buff,
            Func<EffectMakerPerspective, bool> playCondition,
            Func<EffectMakerPerspective, Effect> effect)
        {
            return new SequentialEffectMakerItem(Phase.Play, Zone.Play, buff, playCondition, x => x.Card.Zone == Zone.Play && x.Phase != Phase.Play, effect);
        }

        public static SequentialEffectMakerItem TriggerPlayEffect(bool buff,
            Func<EffectMakerPerspective, bool> playCondition,
            Func<EffectMakerPerspective, Effect> effect)
        {
            return new SequentialEffectMakerItem(Phase.Play, Zone.Play, buff, playCondition, x => false, effect);
        }

        public bool Buff { get; private set; }
        public Func<EffectMakerPerspective, bool> PlayCondition { get; private set; }
        public Func<EffectMakerPerspective, bool> SkipCondition { get; private set; }
        public Func<EffectMakerPerspective, Effect> Effect { get; private set; }

        public SequentialEffectMakerItem(bool buff,
            Func<EffectMakerPerspective, bool> playCondition,
            Func<EffectMakerPerspective, bool> skipCondition,
            Func<EffectMakerPerspective, Effect> effect)
        {
            Buff = buff;
            PlayCondition = playCondition;
            SkipCondition = skipCondition;
            Effect = effect;
        }

        public SequentialEffectMakerItem(Phase phase, Zone zone, bool buff,
            Func<EffectMakerPerspective, bool> playCondition,
            Func<EffectMakerPerspective, bool> skipCondition,
            Func<EffectMakerPerspective, Effect> effects)
        {
            Buff = buff;
            PlayCondition = perspective => perspective.Phase == phase &&
                (perspective.Card == null || perspective.Card.Zone == zone) &&
                playCondition(perspective);
            SkipCondition = skipCondition;
            Effect = effects;
        }

        public SequentialEffectMakerItem(Phase phase, Zone zone, bool buff,
            Func<EffectMakerPerspective, Effect> effects)
        {
            Buff = buff;
            PlayCondition = perspective => perspective.Phase == phase &&
                (perspective.Card == null || perspective.Card.Zone == zone);
            SkipCondition = perspective => false;
            Effect = effects;
        }
    }
}
