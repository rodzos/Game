using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Logic.Effects;

namespace Game.Logic.Interactor.Interactors
{
    public class EffectDisplayer
    {
        public static EffectDisplayer Make<EffectType>(Func<EffectType, string> display)
            where EffectType : Effect
        {
            var type = typeof(EffectType);
            return new EffectDisplayer(type,
                (effect) => {
                    if (!(effect is EffectType))
                        throw new ArgumentException($"Effect is not of type {type}");
                    return display((EffectType)effect);
                }
            );
        }

        public Type EffectType { get; private set; }
        private readonly Func<Effect, string> display;

        public EffectDisplayer(Type effectType, Func<Effect, string> display)
        {
            EffectType = effectType;
            this.display = display;
        }

        public string Display(Effect effect)
        {
            return display(effect);
        }

        public static List<EffectDisplayer> AllDisplayers = new List<EffectDisplayer>
        {
            Make<AttackEffect>(effect => $"Атака {effect.Value}"),
            Make<HealEffect>(effect => $"Лечение {effect.Value}"),
            Make<BlockEffect>(effect => $"Блок {effect.Value}"),
            Make<LoseHealthEffect>(effect => $"Потеряй {effect.Value} здоровья" ),
            Make<LoseMaxHealthEffect>(effect => $"Потеряй {effect.Value} максимального здоровья" ),
            Make<DrawEffect>(effect => {
                if (effect.Success)
                    return "Возьми карту";
                else if (effect.Drawn != null)
                    return $"Возьми карту (не удалось взять карту {effect.Drawn.Name})";
                else
                    return $"Возьми карту (не удалось взять карту)";
            }),
            Make<DiscardEffect>(effect => {
                if (effect.Discarded == null)
                    return "Сбрось карту (ничего не сброшено)";
                else
                    return $"Сбрось карту (сброшено {effect.Discarded.Name})";
            }),
            Make<MoveEffect>(effect => {
                if (effect.Moved == null)
                    return $"Перемести карту из {effect.From} в {effect.To} (ничего не перемещено)";
                else
                    return $"Перемести карту из {effect.From} в {effect.To} (перемещено {effect.Moved.Name})";
            }),
            Make<MoveAllEffect>(effect => $"Перемести карты из {effect.From} в {effect.To} (перемещено {effect.AllMoved.Count} карт)"),
            Make<RevealEffect>(effect => {
                if (effect.Revealed == null)
                    return $"Перемести карту из руки (ничего не показано)";
                else
                    return $"Покажи карту из руки (показано {effect.Revealed.Name})";
            }),
            Make<UnhideEffect>(effect => "Открой эту карту"),
            Make<StartInDeckEffect>(effect => null),
            Make<AutoEffect>(effect => null),
            Make<EndEffect>(effect => null),
            Make<FinishTurnEffect>(effect => null),
            Make<HideEffect>(effect => null),
            Make<Effect>(effect => "Неизвестный эффект")
        };
    }
}
