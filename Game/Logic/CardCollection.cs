using Game.Logic.EffectMakers;
using Game.Logic.EffectMakers.CardEffectMakers;
using Game.Logic.Effects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic
{
    public static class CardCollection
    {
        public static CardType Attack = new CardType("Атака",
            "Атака 15",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(15)),
                SequentialEffectMakerItem.End()
            ));

        public static CardType Heal = new CardType("Лечение",
            "Лечение 15",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.PlayEffect(true, x => new HealEffect(15)),
                SequentialEffectMakerItem.End()
            ));

        public static CardType Block = new CardType("Блок",
            "Блок 25",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.PlayEffect(true, x => new BlockEffect(25)),
                SequentialEffectMakerItem.End()
            ));

        public static CardType Bite = new CardType("Укус",
            "Авто: Атака 10, Возьми карту.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.Auto(),
                SequentialEffectMakerItem.PlayEffect(true, x => new DrawEffect()),
                SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(10)),
                SequentialEffectMakerItem.End()
            ), Tag.Auto);

        public static CardType PointBlank = new CardType("В упор",
            "Атака 10. Если Атакуешь хотя бы на 20: еще Атака 10.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(10)),
                SequentialEffectMakerItem.ConditionalPlayEffect(false, x => x.PrevEffects.CountAttackTaken(x.Opponent) >= 20, x => new AttackEffect(10)),
                SequentialEffectMakerItem.End()
            ));

        public static CardType Bullseye = new CardType("В яблочко",
            "Атака 5. Если Атакуешь хотя бы на 25: еще Атака 20.",
            Rarity.Rare, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(5)),
                SequentialEffectMakerItem.ConditionalPlayEffect(false, x => x.PrevEffects.CountAttackTaken(x.Opponent) >= 25, x => new AttackEffect(20)),
                SequentialEffectMakerItem.End()
            ));

        public static CardType InSight = new CardType("На мушке",
            "В следующий ход: Атака 15.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.NextTurn(),
                SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(15)),
                SequentialEffectMakerItem.End()
            ));

        public static CardType PoisonArrow = new CardType("Отравленная стрела",
            "В следующие два хода: Атака 10.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.NextTurn(),
                SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(10)),
                SequentialEffectMakerItem.NextTurn(),
                SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(10)),
                SequentialEffectMakerItem.End()
            ));

        public static CardType Berserk = new CardType("Берсерк",
            "Атака 5. Лечение на столько, сколько Атаковал в этот ход.",
            Rarity.Rare, () => new BerserkEffectMaker());

        public static CardType MindReading = new CardType("Чтение мыслей",
            "Атака 5. Лечение 5. Разыграй копию случайной карты из руки противника.",
            Rarity.Rare, () => new MindReadingEffectMaker());

        public static CardType Sabotage = new CardType("Саботаж",
            "Возьми карту. Противник: Сбрось карту.",
            Rarity.Rare, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.PlayEffect(true, x => new DrawEffect()),
                SequentialEffectMakerItem.PlayEffect(false, x => new DiscardEffect()),
                SequentialEffectMakerItem.End()
            ));

        public static CardType Blessing = new CardType("Благословение",
            "Лечение 10. Возьми карту",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.PlayEffect(true, x => new HealEffect(10)),
                SequentialEffectMakerItem.PlayEffect(true, x => new DrawEffect()),
                SequentialEffectMakerItem.End()
            ));

        public static CardType PotionSip = new CardType("Глоток Зелья",
            "Авто: Лечение 10, Возьми карту.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.Auto(),
                SequentialEffectMakerItem.PlayEffect(true, x => new DrawEffect()),
                SequentialEffectMakerItem.PlayEffect(true, x => new HealEffect(10)),
                SequentialEffectMakerItem.End()
            ), Tag.Auto);

        public static CardType Glory = new CardType("Слава",
            "Лечение 5 за каждую карту в руке, кроме одной.",
            Rarity.Rare, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.PlayEffect(true, x => new HealEffect(Math.Max(x.Player.Hand.Count - 1, 0) * 5)),
                SequentialEffectMakerItem.End()
            ));

        public static CardType MightyStrike = new CardType("Навались!",
            "Атака 5 за каждую карту в руке, кроме одной.",
            Rarity.Rare, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(Math.Max(x.Player.Hand.Count - 1, 0) * 5)),
                // TODO counting the amount of cards should be a buff, check all cards
                SequentialEffectMakerItem.End()
            ));

        public static CardType FinisherMove = new CardType("Добивание",
            "Атака 10 за каждую карту в руке минус 10 за каждую карту в руке противника.",
            Rarity.Rare, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(Math.Max(x.Player.Hand.Count - x.Opponent.Hand.Count, 0) * 10)),
                SequentialEffectMakerItem.End()
            ));

        public static CardType Universality = new CardType("Универсальность",
            "Атака 5, Лечение 5, Блок 5, Сбрось карту, Возьми карту.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.PlayEffect(true, x => new HealEffect(5)),
                SequentialEffectMakerItem.PlayEffect(true, x => new BlockEffect(5)),
                SequentialEffectMakerItem.PlayEffect(true, x => new DiscardEffect()),
                SequentialEffectMakerItem.PlayEffect(true, x => new DrawEffect()),
                SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(5)),
                SequentialEffectMakerItem.End()
            ));

        public static CardType Wrestle = new CardType("Схватка",
            "Если у тебя больше карт в руке, чем у противника: Атака 20, иначе: Атака 10.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(x.Player.Hand.Count > x.Opponent.Hand.Count ? 20 : 10)),
                SequentialEffectMakerItem.End()
            ));

        public static CardType CursedFlame = new CardType("Проклятое пламя",
            "Атака 20. Оба игрока Сбрасывают карту.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.PlayEffect(true, x => new DiscardEffect()),
                SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(20)),
                SequentialEffectMakerItem.PlayEffect(false, x => new DiscardEffect()),
                SequentialEffectMakerItem.End()
            ));

        public static CardType Stealth = new CardType("Маскировка",
            "В этот и в следующий ход: Блок 15",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.PlayEffect(true, x => new BlockEffect(15)),
                SequentialEffectMakerItem.NextTurn(),
                SequentialEffectMakerItem.PlayEffect(true, x => new BlockEffect(15)),
                SequentialEffectMakerItem.End()
            ));

        public static CardType Sacrifice = new CardType("Жертвоприношение",
            "Получи 25 урона. В следующий ход: Лечение 50.",
            Rarity.Rare, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.PlayEffect(true, x => new LoseHealthEffect(25)),
                SequentialEffectMakerItem.NextTurn(),
                SequentialEffectMakerItem.PlayEffect(true, x => new HealEffect(50)),
                SequentialEffectMakerItem.End()
            ));

        public static CardType Preparation = new CardType("Подготовка",
            "Возьми карту. В следующий ход: Атака 10.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.PlayEffect(true, x => new DrawEffect()),
                SequentialEffectMakerItem.NextTurn(),
                SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(10)),
                SequentialEffectMakerItem.End()
            ));

        public static CardType Ressurect = new CardType("Воскрешение",
            "Лечение 10. Возьми случайную карту из сброса.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.PlayEffect(true, x => new HealEffect(10)),
                SequentialEffectMakerItem.PlayEffect(true, x => new MoveRandomEffect(Zone.Pile, Zone.Hand)),
                SequentialEffectMakerItem.End()
            ));

        public static CardType BattleSpirit = new CardType("Боевой дух",
            "Лечение 5. Атака 5. Возьми карту",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.PlayEffect(true, x => new HealEffect(5)),
                SequentialEffectMakerItem.PlayEffect(true, x => new DrawEffect()),
                SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(5)),
                SequentialEffectMakerItem.End()
            ));

        public static CardType Spark = new CardType("Искра",
            "В следующие два хода: разыграй верхнюю карту из колоды.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.NextTurn(),
                SequentialEffectMakerItem.PlayEffect(true, x => new MoveFirstEffect(Zone.Deck, Zone.Play)),
                SequentialEffectMakerItem.NextTurn(),
                SequentialEffectMakerItem.PlayEffect(true, x => new MoveFirstEffect(Zone.Deck, Zone.Play)),
                SequentialEffectMakerItem.End()
            ));

        public static CardType Mastery = new CardType("Мастерство",
            "Супер: каждый ход: Атака 5, Блок 5.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.Super(),
                SequentialEffectMakerItem.PlayEffect(true, x => new BlockEffect(5)),
                SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(5)),
                SequentialEffectMakerItem.NextTurn()
            ), Tag.Super);

        public static CardType LifeSteal = new CardType("Кража жизни",
            "Атака 10. Лечение 10",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.PlayEffect(true, x => new HealEffect(10)),
                SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(10)),
                SequentialEffectMakerItem.End()
            ));

        public static CardType MassHeal = new CardType("Массовое исцеление",
            "Лечение 25. Противник: Лечение 10.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.PlayEffect(true, x => new HealEffect(25)),
                SequentialEffectMakerItem.PlayEffect(false, x => new HealEffect(10)),
                SequentialEffectMakerItem.End()
            ));

        public static CardType EnchantedCards = new CardType("Зачарованные карты",
            "Разыграй 2 случайные карты из руки.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.PlayEffect(true, x => new MoveRandomEffect(Zone.Hand, Zone.Play)),
                SequentialEffectMakerItem.PlayEffect(true, x => new MoveRandomEffect(Zone.Hand, Zone.Play)),
                SequentialEffectMakerItem.End()
            ));

        public static CardType DevouringFlame = new CardType("Поглощающее пламя",
            "Атака 25. Получи 10 урона.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.PlayEffect(true, x => new LoseHealthEffect(10)),
                SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(25)),
                SequentialEffectMakerItem.End()
            ));

        public static CardType TakeAim = new CardType("Прицел",
            "Ловушка: в следующий раз, когда ты Атакуешь хотя бы на 15 урона: Атака 15.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.Hidden(),
                SequentialEffectMakerItem.NextTurn(),
                SequentialEffectMakerItem.TriggerPlayEffect(false, x => x.PrevEffects.CountAttackTaken(x.Opponent) >= 15, x => new UnhideEffect()),
                SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(15)),
                SequentialEffectMakerItem.End()
            ), Tag.Trap);

        public static CardType Regeneration = new CardType("Регенерация",
            "Ловушка: когда здоровье становится меньше 50 и больше 0: Лечение до 50 здоровья.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.Hidden(),
                SequentialEffectMakerItem.NextTurn(),
                SequentialEffectMakerItem.TriggerPlayEffect(true, x => x.Player.Health < 50 && x.Player.Health > 0, x => new UnhideEffect()),
                SequentialEffectMakerItem.PlayEffect(true, x => new HealEffect(Math.Max(50 - x.Player.Health, 0))),
                SequentialEffectMakerItem.End()
            ), Tag.Trap);

        public static CardType WeaponCrate = new CardType("Ящик с оружием",
            "Разыграй редкую карту из колоды.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.PlayEffect(true, x => new MoveMatchingEffect(Zone.Deck, Zone.Play, y => y.Rarity == Rarity.Rare)),
                SequentialEffectMakerItem.End()
            ));

        public static CardType Combustion = new CardType("Самовозгорание",
            "Ловушка: когда противник Берет карту в результате действия своей карты: взятая карта Сбрасывается, Атака 15.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.Hidden(),
                SequentialEffectMakerItem.NextTurn(),
                SequentialEffectMakerItem.TriggerPlayEffect(false, x => x.PrevEffects
                    .Any(y => y.Caller == x.Opponent && y.Effect is DrawEffect && ((DrawEffect)y.Effect).Success), x => new UnhideEffect()),
                SequentialEffectMakerItem.PlayEffect(false, x => new DiscardMatchingEffect(y => y == ((DrawEffect)x.PrevEffects
                    .FirstOrDefault(z => z.Caller == x.Opponent && z.Effect is DrawEffect).Effect).Drawn)),
                SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(15)),
                SequentialEffectMakerItem.End()
            ), Tag.Trap);

        public static CardType Copycat = new CardType("Клонирование",
            "Копирует эффекты до трех других своих карт в зоне игры, действующие в этот ход.",
            Rarity.Rare, () => new CopycatEffectMaker());

        public static CardType Robbery = new CardType("Грабеж",
            "Атака 5. Противник Берет карту, показывает и Сбрасывает ее. Разыграй ее копию.",
            Rarity.Rare, () => new RobberyEffectMaker());

        public static CardType Golem = new CardType("Голем",
            "Авто: Блок 20, Возьми карту.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.Auto(),
                SequentialEffectMakerItem.PlayEffect(true, x => new DrawEffect()),
                SequentialEffectMakerItem.PlayEffect(true, x => new BlockEffect(20)),
                SequentialEffectMakerItem.End()
            ), Tag.Auto);

        public static CardType LivingBook = new CardType("Ожившая книга",
            "Авто: Возьми 2 карты.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.Auto(),
                SequentialEffectMakerItem.PlayEffect(true, x => new DrawEffect()),
                SequentialEffectMakerItem.PlayEffect(true, x => new DrawEffect()),
                SequentialEffectMakerItem.End()
            ), Tag.Auto);

        public static CardType Plague = new CardType("Чума",
            "Авто: уменьшает максимальное здоровье противника на 5, Возьми карту.",
            Rarity.Rare, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.Auto(),
                SequentialEffectMakerItem.PlayEffect(true, x => new DrawEffect()),
                SequentialEffectMakerItem.PlayEffect(false, x => new LoseMaxHealthEffect(5)),
                SequentialEffectMakerItem.End()
            ), Tag.Auto);

        public static CardType ForkedLightning = new CardType("Раздвоенная молния",
            "Атака 15. Возьми Раздвоенную молнию из колоды.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.PlayEffect(true, x => new DrawMatchingEffect(y => y.Type == ForkedLightning)),
                SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(15)),
                SequentialEffectMakerItem.End()
            ));

        public static CardType Stun = new CardType("Оглушение",
            "Атака 10. Противник Сбрасывает карту.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(10)),
                SequentialEffectMakerItem.PlayEffect(false, x => new DiscardEffect()),
                SequentialEffectMakerItem.End()
            ));

        public static CardType MindCorruption = new CardType("Искажение разума",
            "Атака 5 за каждую карту в руке противника.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(x.Opponent.Hand.Count * 5)),
                SequentialEffectMakerItem.End()
            ));

        public static CardType Treasure = new CardType("Сокровища",
            "Возьми карты до 3 карт в руке.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.ConditionalPlayEffect(true, x => x.Player.Hand.Count < 3, x => new DrawEffect()),
                SequentialEffectMakerItem.ConditionalPlayEffect(true, x => x.Player.Hand.Count < 3, x => new DrawEffect()),
                SequentialEffectMakerItem.ConditionalPlayEffect(true, x => x.Player.Hand.Count < 3, x => new DrawEffect()),
                SequentialEffectMakerItem.End()
            ));

        public static CardType HealingRain = new CardType("Целебный ливень",
            "Лечение 10. Разыграй все копии этой карты из руки и колоды.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.PlayEffect(true, x => new HealEffect(10)),
                SequentialEffectMakerItem.PlayEffect(true, x => new MoveAllMatchingEffect(Zone.Hand, Zone.Play, y => y.Type == HealingRain)),
                SequentialEffectMakerItem.PlayEffect(true, x => new MoveAllMatchingEffect(Zone.Deck, Zone.Play, y => y.Type == HealingRain)),
                SequentialEffectMakerItem.End()
            ));

        public static CardType Bomb = new CardType("Бомба",
            "Атака 15. Отложи эту карту.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(15)),
                SequentialEffectMakerItem.PlayEffect(true, x => new MoveEffect(Zone.Play, Zone.PutOff)),
                SequentialEffectMakerItem.End()
            ));

        public static CardType SorcerrersAura = new CardType("Чародейская аура",
            "Этот и два следующих хода: если противник Атакует: Возьми карту.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.ConditionalPlayEffect(true, x => x.PrevEffects.CountAttackTaken(x.Player) > 0, x => new DrawEffect()),
                SequentialEffectMakerItem.NextTurn(),
                new SequentialEffectMakerItem(Phase.Play, Zone.Play, false, x => null),
                SequentialEffectMakerItem.ConditionalPlayEffect(true, x => x.PrevEffects.CountAttackTaken(x.Player) > 0, x => new DrawEffect()),
                SequentialEffectMakerItem.NextTurn(),
                new SequentialEffectMakerItem(Phase.Play, Zone.Play, false, x => null),
                SequentialEffectMakerItem.ConditionalPlayEffect(true, x => x.PrevEffects.CountAttackTaken(x.Player) > 0, x => new DrawEffect()),
                SequentialEffectMakerItem.End()
            ));

        public static CardType RepellingAura = new CardType("Отбрасывающая аура",
            "Этот и два следующих хода: если противник Атакует: Атака 5 и Блок 5.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.ConditionalPlayEffect(true, x => x.PrevEffects.CountAttackTaken(x.Player) > 0, x => new BlockEffect(5)),
                SequentialEffectMakerItem.ConditionalPlayEffect(false, x => x.PrevEffects.CountAttackTaken(x.Player) > 0, x => new AttackEffect(5)),
                SequentialEffectMakerItem.NextTurn(),
                new SequentialEffectMakerItem(Phase.Play, Zone.Play, true, x => null),
                SequentialEffectMakerItem.ConditionalPlayEffect(true, x => x.PrevEffects.CountAttackTaken(x.Player) > 0, x => new BlockEffect(5)),
                SequentialEffectMakerItem.ConditionalPlayEffect(false, x => x.PrevEffects.CountAttackTaken(x.Player) > 0, x => new AttackEffect(5)),
                SequentialEffectMakerItem.NextTurn(),
                new SequentialEffectMakerItem(Phase.Play, Zone.Play, true, x => null),
                SequentialEffectMakerItem.ConditionalPlayEffect(true, x => x.PrevEffects.CountAttackTaken(x.Player) > 0, x => new BlockEffect(5)),
                SequentialEffectMakerItem.ConditionalPlayEffect(false, x => x.PrevEffects.CountAttackTaken(x.Player) > 0, x => new AttackEffect(5)),
                SequentialEffectMakerItem.End()
            ));

        public static CardType DefensiveAura = new CardType("Защитная аура",
            "Этот и два следующих хода: если противник Атакует: Блок 10.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.ConditionalPlayEffect(true, x => x.PrevEffects.CountAttackTaken(x.Player) > 0, x => new BlockEffect(10)),
                SequentialEffectMakerItem.NextTurn(),
                new SequentialEffectMakerItem(Phase.Play, Zone.Play, false, x => null),
                SequentialEffectMakerItem.ConditionalPlayEffect(true, x => x.PrevEffects.CountAttackTaken(x.Player) > 0, x => new BlockEffect(10)),
                SequentialEffectMakerItem.NextTurn(),
                new SequentialEffectMakerItem(Phase.Play, Zone.Play, false, x => null),
                SequentialEffectMakerItem.ConditionalPlayEffect(true, x => x.PrevEffects.CountAttackTaken(x.Player) > 0, x => new BlockEffect(10)),
                SequentialEffectMakerItem.End()
            ));

        public static CardType IceAura = new CardType("Ледяная аура",
            "Этот и два следующих хода: если противник Атакует: противник кладет случайную карту из руки на верх колоды.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.ConditionalPlayEffect(false, x => x.PrevEffects.CountAttackTaken(x.Player) > 0, x => new FreezeEffect()),
                SequentialEffectMakerItem.NextTurn(),
                new SequentialEffectMakerItem(Phase.Play, Zone.Play, false, x => null),
                SequentialEffectMakerItem.ConditionalPlayEffect(false, x => x.PrevEffects.CountAttackTaken(x.Player) > 0, x => new FreezeEffect()),
                SequentialEffectMakerItem.NextTurn(),
                new SequentialEffectMakerItem(Phase.Play, Zone.Play, false, x => null),
                SequentialEffectMakerItem.ConditionalPlayEffect(false, x => x.PrevEffects.CountAttackTaken(x.Player) > 0, x => new FreezeEffect()),
                SequentialEffectMakerItem.End()
            ));

        public static CardType ChillingTouch = new CardType("Ледяное касание",
            "Атака 10. Противник кладет случайную карту из руки на верх колоды.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(10)),
                SequentialEffectMakerItem.PlayEffect(false, x => new FreezeEffect()),
                SequentialEffectMakerItem.End()
            ));

        public static CardType Freeze = new CardType("Заморозка",
            "Противник кладет две случайные карты из руки на верх своей колоды.",
            Rarity.Rare, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.PlayEffect(false, x => new FreezeEffect()),
                SequentialEffectMakerItem.PlayEffect(false, x => new FreezeEffect()),
                SequentialEffectMakerItem.End()
            ));

        public static CardType GraveDigging = new CardType("Раскопка могилы",
            "Разыграй случайную карту из сброса.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.PlayEffect(true, x => new MoveRandomEffect(Zone.Pile, Zone.Play)),
                SequentialEffectMakerItem.End()
            ));

        public static CardType Evolution = new CardType("Эволюция",
            "Атака 10. Каждое следующее применение этой карты наносит на 5 урона больше.",
            Rarity.Rare, () => new EvolutionEffectMaker());

        public static CardType FireTank = new CardType("Огненный танк",
            "Атака 10. В свой следующий пропуск хода: Атака 15.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(10)),
                SequentialEffectMakerItem.TriggerPlayEffect(false, x => x.Player.Played == null, x => new AttackEffect(15)),
                SequentialEffectMakerItem.End()
            ));

        public static CardType RepairBot = new CardType("Ремонтный бот",
            "Лечение 10, Блок 5. В свой следующий пропуск хода: Лечение 10, Блок 5.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.PlayEffect(true, x => new HealEffect(10)),
                SequentialEffectMakerItem.PlayEffect(true, x => new BlockEffect(5)),
                SequentialEffectMakerItem.TriggerPlayEffect(true, x => x.Player.Played == null, x => new HealEffect(10)),
                SequentialEffectMakerItem.PlayEffect(true, x => new BlockEffect(5)),
                SequentialEffectMakerItem.End()
            ));

        public static CardType HomingChicken = new CardType("Самонаводящаяся курица",
            "Сбрось карту. Атака 15. В свой следующий пропуск хода: Атака 20.",
            Rarity.Rare, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.PlayEffect(true, x => new DiscardEffect()),
                SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(15)),
                SequentialEffectMakerItem.TriggerPlayEffect(false, x => x.Player.Played == null, x => new AttackEffect(20)),
                SequentialEffectMakerItem.End()
            ));

        public static CardType Thunder = new CardType("Гроза",
            "Атака 15. Если в руке есть Гроза: покажи ее, Атака 5.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(15)),
                SequentialEffectMakerItem.PlayEffect(true, x => new RevealMatchingEffect(Zone.Hand, y => y.Type == Thunder)),
                SequentialEffectMakerItem.ConditionalPlayEffect(false, x => x.PrevEffects.EffectsFromCard(x.Card)
                    .Any(y => y.Effect is RevealEffect && ((RevealEffect)y.Effect).Revealed != null), x => new AttackEffect(5)),
                SequentialEffectMakerItem.End()
            ));

        public static CardType Fireball = new CardType("Огненный шар",
            "Атака 15. Если рука не пустая: Сбрось карту, еще Атака 10.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.PlayEffect(true, x => new DiscardEffect()),
                SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(15)),
                SequentialEffectMakerItem.ConditionalPlayEffect(false, x => x.PrevEffects.EffectsFromCard(x.Card)
                    .Any(y => y.Effect is DiscardEffect && ((DiscardEffect)y.Effect).Discarded != null), x => new AttackEffect(10)),
                SequentialEffectMakerItem.End()
            ));

        public static CardType FlameRitual = new CardType("Ритуал сожжения",
            "Возьми 3 карты и Сбрось их. Лечение 30",
            Rarity.Rare, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.PlayEffect(true, x => new DrawEffect()),
                SequentialEffectMakerItem.PlayEffect(true, x => new DrawEffect()),
                SequentialEffectMakerItem.PlayEffect(true, x => new DrawEffect()),
                SequentialEffectMakerItem.PlayEffect(true, x => new DiscardMatchingEffect(y =>
                    x.PrevEffects.EffectsFromCard(x.Card).Any(z => z.Effect is DrawEffect && y == ((DrawEffect)z.Effect).Drawn) &&
                    !x.PrevEffects.EffectsFromCard(x.Card).Any(z => z.Effect is DiscardEffect && y == ((DiscardEffect)z.Effect).Discarded))),
                SequentialEffectMakerItem.PlayEffect(true, x => new DiscardMatchingEffect(y =>
                    x.PrevEffects.EffectsFromCard(x.Card).Any(z => z.Effect is DrawEffect && y == ((DrawEffect)z.Effect).Drawn) &&
                    !x.PrevEffects.EffectsFromCard(x.Card).Any(z => z.Effect is DiscardEffect && y == ((DiscardEffect)z.Effect).Discarded))),
                SequentialEffectMakerItem.PlayEffect(true, x => new DiscardMatchingEffect(y =>
                    x.PrevEffects.EffectsFromCard(x.Card).Any(z => z.Effect is DrawEffect && y == ((DrawEffect)z.Effect).Drawn) &&
                    !x.PrevEffects.EffectsFromCard(x.Card).Any(z => z.Effect is DiscardEffect && y == ((DiscardEffect)z.Effect).Discarded))),
                SequentialEffectMakerItem.PlayEffect(true, x => new HealEffect(30)),
                SequentialEffectMakerItem.End()
            ));

        public static CardType Permafrost = new CardType("Вечная мерзлота",
            "Блок 100. Противник: Блок 100.",
            Rarity.Rare, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.PlayEffect(true, x => new BlockEffect(100)),
                SequentialEffectMakerItem.PlayEffect(false, x => new BlockEffect(100)),
                SequentialEffectMakerItem.End()
            ));

        public static CardType CurseOfFear = new CardType("Заклятие ужаса",
            "Атака 10. За каждые 15 Атаки: противник Сбрасывает карту.",
            Rarity.Common, () => new CurseOfFearEffectMaker());

        public static CardType SpikeTrap = new CardType("Ловушка с шипами",
            "Ловушка: когда противник Атакует, Атака на такое же количество урона.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.Hidden(),
                SequentialEffectMakerItem.NextTurn(),
                SequentialEffectMakerItem.TriggerPlayEffect(false, x => x.PrevEffects.CountAttackTaken(x.Player) > 0, x => new UnhideEffect()),
                new SequentialEffectMakerItem(false,
                    x => (x.Card == null || x.Card.Zone == Zone.Play) && x.Phase == Phase.Play && x.PrevEffects.CountAttackTaken(x.Player) >
                        x.PrevEffects.EffectsFromCard(x.Card).Where(y => y.Effect is AttackEffect).Sum(y => ((AttackEffect)y.Effect).Value),
                    x => !(x.Card == null || x.Card.Zone == Zone.Play) || x.Phase != Phase.Play,
                    x => new AttackEffect(x.PrevEffects.CountAttackTaken(x.Player) -
                        x.PrevEffects.EffectsFromCard(x.Card).Where(y => y.Effect is AttackEffect).Sum(y => ((AttackEffect)y.Effect).Value))),
                SequentialEffectMakerItem.End()
            ));

        public static CardType CircularSaw = new CardType("Циркулярная пила",
            "Атака 15. Когда эта карта Сбрасывается с руки: разыграй эту карту.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                new SequentialEffectMakerItem(true,
                    x => x.PrevEffects.Any(y => y.Effect is DiscardEffect && ((DiscardEffect)y.Effect).Discarded == x.Card),
                    x => x.Card.Zone == Zone.Play, x => new MoveEffect(x.Card.Zone, Zone.Play)),
                SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(15)),
                SequentialEffectMakerItem.End()
            ));

        public static CardType Clone = new CardType("Призыв клона",
            "Блок 10, Возьми карту. Пока эта карта в руке, когда другая карта Сбрасывается с руки: верни ту карту в руку, разыграй эту карту.",
            Rarity.Common, () => new CloneEffectMaker());

        public static CardType Thunderstorm = new CardType("Шторм",
            "Возьми карту. В следующие два хода: разыграй Искру из руки.",
            Rarity.Rare, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.PlayEffect(true, x => new DrawEffect()),
                SequentialEffectMakerItem.NextTurn(),
                SequentialEffectMakerItem.PlayEffect(true, x => new MoveMatchingEffect(Zone.Hand, Zone.Play, y => y.Type == Spark)),
                SequentialEffectMakerItem.NextTurn(),
                SequentialEffectMakerItem.PlayEffect(true, x => new MoveMatchingEffect(Zone.Hand, Zone.Play, y => y.Type == Spark)),
                SequentialEffectMakerItem.End()
            ));

        public static CardType Scout = new CardType("Разведка",
            "Блок 15. Если противник Атакует: Возьми карту.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.PlayEffect(true, x => new BlockEffect(15)),
                SequentialEffectMakerItem.ConditionalPlayEffect(true, x => x.PrevEffects.CountAttackTaken(x.Player) > 0, x => new DrawEffect()),
                SequentialEffectMakerItem.End()
            ));

        public static CardType FirstAid = new CardType("Первая помощь",
            "Лечение 5. Блок 10. Возьми карту с эффектом \"Лечение\" из колоды.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.PlayEffect(true, x => new HealEffect(5)),
                SequentialEffectMakerItem.PlayEffect(true, x => new BlockEffect(10)),
                SequentialEffectMakerItem.PlayEffect(true, x => new DrawMatchingEffect(
                    y => y.Type.CallIsolated(x.Game, x.Random).Any(z => z.Effect is HealEffect))),
                SequentialEffectMakerItem.End()
            ));

        public static CardType Combo = new CardType("Комбо",
            "Возьми и покажи карту. Если эта карта имеет эффект \"Атака\": Атака 15.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.StartInDeck(),
                SequentialEffectMakerItem.PlayEffect(true, x => new DrawEffect()),
                SequentialEffectMakerItem.ConditionalPlayEffect(false, x => x.PrevEffects.EffectsFromCard(x.Card)
                    .Any(y => y.Effect is DrawEffect && ((DrawEffect)y.Effect).Drawn != null &&
                    ((DrawEffect)y.Effect).Drawn.Type.CallIsolated(
                        x.Game, x.Random).Any(z => z.Effect is AttackEffect)),
                    x => new AttackEffect(15)),
                SequentialEffectMakerItem.End()
            ));

        public static CardType CardsOfFate = new CardType("Судьбоносные карты",
            "Супер: разыграй все карты из руки.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.Super(),
                SequentialEffectMakerItem.PlayEffect(true, x => new MoveAllEffect(Zone.Hand, Zone.Play)),
                SequentialEffectMakerItem.End()
            ), Tag.Super);

        public static CardType TrappedCorridor = new CardType("Коридор ловушек",
            "Возьми ловушку из колоды. Затем, разыграй ловушку из руки.",
            Rarity.Common, () => new SequentialEffectMaker(
                SequentialEffectMakerItem.Super(),
                SequentialEffectMakerItem.PlayEffect(true, x => new DrawMatchingEffect(y => y.Tags.Contains(Tag.Trap))),
                SequentialEffectMakerItem.PlayEffect(true, x => new MoveRandomMatchingEffect(Zone.Hand, Zone.Play, y => y.Tags.Contains(Tag.Trap))),
                SequentialEffectMakerItem.End()
            ));

        public static ReadOnlyCollection<CardType> AllCards = new ReadOnlyCollection<CardType>(new List<CardType>
        {
            Attack, Heal, Block, Bite, PointBlank, Bullseye, InSight, PoisonArrow, Berserk, MindReading, Sabotage, Blessing, PotionSip,
            Glory, MightyStrike, FinisherMove, Universality, Wrestle, CursedFlame, Stealth, Sacrifice, Preparation, Ressurect,
            BattleSpirit, Spark, Mastery, LifeSteal, MassHeal, EnchantedCards, DevouringFlame, TakeAim, Regeneration, WeaponCrate,
            Combustion, Copycat, Robbery, Golem, LivingBook, Plague, ForkedLightning, Stun, MindCorruption, Treasure, HealingRain,
            Bomb, SorcerrersAura, RepellingAura, DefensiveAura, IceAura, ChillingTouch, Freeze, GraveDigging, Evolution,
            FireTank, RepairBot, HomingChicken, Thunder, Fireball, FlameRitual, Permafrost, CurseOfFear, SpikeTrap, CircularSaw, Clone,
            Thunderstorm, Scout, Combo, FirstAid, CardsOfFate, TrappedCorridor
        });
    }
}
