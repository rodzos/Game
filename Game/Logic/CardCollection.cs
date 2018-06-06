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
        public static CardType Attack = new CardType("Атака", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(15)),
            SequentialEffectMakerItem.End()
        ));

        public static CardType Heal = new CardType("Лечение", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.PlayEffect(true, x => new HealEffect(15)),
            SequentialEffectMakerItem.End()
        ));

        public static CardType Block = new CardType("Блок", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.PlayEffect(true, x => new BlockEffect(25)),
            SequentialEffectMakerItem.End()
        ));

        public static CardType Bite = new CardType("Укус", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.Auto(),
            SequentialEffectMakerItem.PlayEffect(true, x => new DrawEffect()),
            SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(10)),
            SequentialEffectMakerItem.End()
        ), Tag.Auto);

        public static CardType PointBlank = new CardType("В упор", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(10)),
            SequentialEffectMakerItem.ConditionalPlayEffect(false, x => x.PrevEffects.CountAttackTaken(x.Opponent) >= 20, x => new AttackEffect(10)),
            SequentialEffectMakerItem.End()
        ));

        public static CardType Bullseye = new CardType("В яблочко", Rarity.Rare, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(5)),
            SequentialEffectMakerItem.ConditionalPlayEffect(false, x => x.PrevEffects.CountAttackTaken(x.Opponent) >= 25, x => new AttackEffect(20)),
            SequentialEffectMakerItem.End()
        ));

        public static CardType InSight = new CardType("На мушке", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.NextTurn(),
            SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(15)),
            SequentialEffectMakerItem.End()
        ));

        public static CardType PoisonArrow = new CardType("Отравленная стрела", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.NextTurn(),
            SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(10)),
            SequentialEffectMakerItem.NextTurn(),
            SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(10)),
            SequentialEffectMakerItem.End()
        ));

        public static CardType Berserk = new CardType("Берсерк", Rarity.Rare, () => new BerserkEffectMaker());

        public static CardType MindReading = new CardType("Чтение мыслей", Rarity.Rare, () => new MindReadingEffectMaker());

        public static CardType Sabotage = new CardType("Саботаж", Rarity.Rare, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.PlayEffect(true, x => new DrawEffect()),
            SequentialEffectMakerItem.PlayEffect(false, x => new DiscardEffect()),
            SequentialEffectMakerItem.End()
        ));

        public static CardType Blessing = new CardType("Благословение", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.PlayEffect(true, x => new HealEffect(10)),
            SequentialEffectMakerItem.PlayEffect(true, x => new DrawEffect()),
            SequentialEffectMakerItem.End()
        ));

        public static CardType PotionSip = new CardType("Глоток Зелья", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.Auto(),
            SequentialEffectMakerItem.PlayEffect(true, x => new DrawEffect()),
            SequentialEffectMakerItem.PlayEffect(true, x => new HealEffect(10)),
            SequentialEffectMakerItem.End()
        ), Tag.Auto);

        public static CardType Glory = new CardType("Слава", Rarity.Rare, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.PlayEffect(true, x => new HealEffect(Math.Max(x.Player.Hand.Count - 1, 0) * 5)),
            SequentialEffectMakerItem.End()
        ));

        public static CardType MightyStrike = new CardType("Навались!", Rarity.Rare, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(Math.Max(x.Player.Hand.Count - 1, 0) * 5)),
            // TODO counting the amount of cards should be a buff, check all cards
            SequentialEffectMakerItem.End()
        ));

        public static CardType FinisherMove = new CardType("Добивание", Rarity.Rare, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(Math.Max(x.Player.Hand.Count - x.Opponent.Hand.Count, 0) * 10)),
            SequentialEffectMakerItem.End()
        ));

        public static CardType Universality = new CardType("Универсальность", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.PlayEffect(true, x => new HealEffect(5)),
            SequentialEffectMakerItem.PlayEffect(true, x => new BlockEffect(5)),
            SequentialEffectMakerItem.PlayEffect(true, x => new DiscardEffect()),
            SequentialEffectMakerItem.PlayEffect(true, x => new DrawEffect()),
            SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(5)),
            SequentialEffectMakerItem.End()
        ));

        public static CardType Wrestle = new CardType("Схватка", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(x.Player.Hand.Count > x.Opponent.Hand.Count ? 20 : 10)),
            SequentialEffectMakerItem.End()
        ));

        public static CardType CursedFlame = new CardType("Проклятое пламя", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.PlayEffect(true, x => new DiscardEffect()),
            SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(20)),
            SequentialEffectMakerItem.PlayEffect(false, x => new DiscardEffect()),
            SequentialEffectMakerItem.End()
        ));

        public static CardType Stealth = new CardType("Маскировка", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.PlayEffect(true, x => new BlockEffect(15)),
            SequentialEffectMakerItem.NextTurn(),
            SequentialEffectMakerItem.PlayEffect(true, x => new BlockEffect(15)),
            SequentialEffectMakerItem.End()
        ));

        public static CardType Sacrifice = new CardType("Жертвоприношение", Rarity.Rare, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.PlayEffect(true, x => new LoseHealthEffect(25)),
            SequentialEffectMakerItem.NextTurn(),
            SequentialEffectMakerItem.PlayEffect(true, x => new HealEffect(50)),
            SequentialEffectMakerItem.End()
        ));

        public static CardType Preparation = new CardType("Подготовка", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.PlayEffect(true, x => new DrawEffect()),
            SequentialEffectMakerItem.NextTurn(),
            SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(10)),
            SequentialEffectMakerItem.End()
        ));

        public static CardType Ressurect = new CardType("Воскрешение", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.PlayEffect(true, x => new HealEffect(10)),
            SequentialEffectMakerItem.PlayEffect(true, x => new MoveRandomEffect(Zone.Pile, Zone.Hand)),
            SequentialEffectMakerItem.End()
        ));

        public static CardType BattleSpirit = new CardType("Боевой дух", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.PlayEffect(true, x => new HealEffect(5)),
            SequentialEffectMakerItem.PlayEffect(true, x => new DrawEffect()),
            SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(5)),
            SequentialEffectMakerItem.End()
        ));

        public static CardType Spark = new CardType("Искра", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.NextTurn(),
            SequentialEffectMakerItem.PlayEffect(true, x => new MoveFirstEffect(Zone.Deck, Zone.Play)),
            SequentialEffectMakerItem.NextTurn(),
            SequentialEffectMakerItem.PlayEffect(true, x => new MoveFirstEffect(Zone.Deck, Zone.Play)),
            SequentialEffectMakerItem.End()
        ));

        public static CardType Mastery = new CardType("Мастерство", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.Super(),
            SequentialEffectMakerItem.PlayEffect(true, x => new BlockEffect(5)),
            SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(5)),
            SequentialEffectMakerItem.NextTurn()
        ), Tag.Super);

        public static CardType LifeSteal = new CardType("Кража жизни", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.PlayEffect(true, x => new HealEffect(10)),
            SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(10)),
            SequentialEffectMakerItem.End()
        ));

        public static CardType MassHeal = new CardType("Массовое исцеление", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.PlayEffect(true, x => new HealEffect(25)),
            SequentialEffectMakerItem.PlayEffect(false, x => new HealEffect(10)),
            SequentialEffectMakerItem.End()
        ));

        public static CardType EnchantedCards = new CardType("Зачарованные карты", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.PlayEffect(true, x => new MoveRandomEffect(Zone.Hand, Zone.Play)),
            SequentialEffectMakerItem.PlayEffect(true, x => new MoveRandomEffect(Zone.Hand, Zone.Play)),
            SequentialEffectMakerItem.End()
        ));

        public static CardType DevouringFlame = new CardType("Поглощающее пламя", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.PlayEffect(true, x => new LoseHealthEffect(10)),
            SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(25)),
            SequentialEffectMakerItem.End()
        ));

        public static CardType TakeAim = new CardType("Прицел", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.Hidden(),
            SequentialEffectMakerItem.NextTurn(),
            SequentialEffectMakerItem.TriggerPlayEffect(false, x => x.PrevEffects.CountAttackTaken(x.Opponent) >= 15, x => new UnhideEffect()),
            SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(15)),
            SequentialEffectMakerItem.End()
        ), Tag.Trap);

        public static CardType Regeneration = new CardType("Регенерация", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.Hidden(),
            SequentialEffectMakerItem.NextTurn(),
            SequentialEffectMakerItem.TriggerPlayEffect(true, x => x.Player.Health < 50 && x.Player.Health > 0, x => new UnhideEffect()),
            SequentialEffectMakerItem.PlayEffect(true, x => new HealEffect(Math.Max(50 - x.Player.Health, 0))),
            SequentialEffectMakerItem.End()
        ), Tag.Trap);

        public static CardType WeaponCrate = new CardType("Ящик с оружием", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.PlayEffect(true, x => new MoveMatchingEffect(Zone.Deck, Zone.Play, y => y.Rarity == Rarity.Rare)),
            SequentialEffectMakerItem.End()
        ));

        public static CardType Combustion = new CardType("Самовозгорание", Rarity.Common, () => new SequentialEffectMaker(
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

        public static CardType Copycat = new CardType("Клонирование", Rarity.Rare, () => new CopycatEffectMaker());

        public static CardType Robbery = new CardType("Грабеж", Rarity.Rare, () => new RobberyEffectMaker());

        public static CardType Golem = new CardType("Голем", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.Auto(),
            SequentialEffectMakerItem.PlayEffect(true, x => new DrawEffect()),
            SequentialEffectMakerItem.PlayEffect(true, x => new BlockEffect(20)),
            SequentialEffectMakerItem.End()
        ), Tag.Auto);

        public static CardType LivingBook = new CardType("Ожившая книга", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.Auto(),
            SequentialEffectMakerItem.PlayEffect(true, x => new DrawEffect()),
            SequentialEffectMakerItem.PlayEffect(true, x => new DrawEffect()),
            SequentialEffectMakerItem.End()
        ), Tag.Auto);

        public static CardType Plague = new CardType("Чума", Rarity.Rare, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.Auto(),
            SequentialEffectMakerItem.PlayEffect(true, x => new DrawEffect()),
            SequentialEffectMakerItem.PlayEffect(false, x => new LoseMaxHealthEffect(5)),
            SequentialEffectMakerItem.End()
        ), Tag.Auto);

        public static CardType ForkedLightning = new CardType("Раздвоенная молния", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.PlayEffect(true, x => new DrawMatchingEffect(y => y.Type == ForkedLightning)),
            SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(15)),
            SequentialEffectMakerItem.End()
        ));

        public static CardType Stun = new CardType("Оглушение", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(10)),
            SequentialEffectMakerItem.PlayEffect(false, x => new DiscardEffect()),
            SequentialEffectMakerItem.End()
        ));

        public static CardType MindCorruption = new CardType("Искажение разума", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(x.Opponent.Hand.Count * 5)),
            SequentialEffectMakerItem.End()
        ));

        public static CardType Treasure = new CardType("Сокровища", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.ConditionalPlayEffect(true, x => x.Player.Hand.Count < 3, x => new DrawEffect()),
            SequentialEffectMakerItem.ConditionalPlayEffect(true, x => x.Player.Hand.Count < 3, x => new DrawEffect()),
            SequentialEffectMakerItem.ConditionalPlayEffect(true, x => x.Player.Hand.Count < 3, x => new DrawEffect()),
            SequentialEffectMakerItem.End()
        ));

        public static CardType HealingRain = new CardType("Целебный ливень", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.PlayEffect(true, x => new HealEffect(10)),
            SequentialEffectMakerItem.PlayEffect(true, x => new MoveAllMatchingEffect(Zone.Hand, Zone.Play, y => y.Type == HealingRain)),
            SequentialEffectMakerItem.PlayEffect(true, x => new MoveAllMatchingEffect(Zone.Deck, Zone.Play, y => y.Type == HealingRain)),
            SequentialEffectMakerItem.End()
        ));

        public static CardType Bomb = new CardType("Бомба", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(15)),
            SequentialEffectMakerItem.PlayEffect(true, x => new MoveEffect(Zone.Play, Zone.PutOff)),
            SequentialEffectMakerItem.End()
        ));

        public static CardType SorcerrersAura = new CardType("Чародейская аура", Rarity.Common, () => new SequentialEffectMaker(
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

        public static CardType RepellingAura = new CardType("Отбрасывающая аура", Rarity.Common, () => new SequentialEffectMaker(
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

        public static CardType DefensiveAura = new CardType("Защитная аура", Rarity.Common, () => new SequentialEffectMaker(
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

        public static CardType IceAura = new CardType("Ледяная аура", Rarity.Common, () => new SequentialEffectMaker(
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

        public static CardType ChillingTouch = new CardType("Ледяное касание", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(10)),
            SequentialEffectMakerItem.PlayEffect(false, x => new FreezeEffect()),
            SequentialEffectMakerItem.End()
        ));

        public static CardType Freeze = new CardType("Заморозка", Rarity.Rare, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.PlayEffect(false, x => new FreezeEffect()),
            SequentialEffectMakerItem.PlayEffect(false, x => new FreezeEffect()),
            SequentialEffectMakerItem.End()
        ));

        public static CardType GraveDigging = new CardType("Раскопка могилы", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.PlayEffect(true, x => new MoveRandomEffect(Zone.Pile, Zone.Play)),
            SequentialEffectMakerItem.End()
        ));

        public static CardType Evolution = new CardType("Эволюция", Rarity.Rare, () => new EvolutionEffectMaker());

        public static CardType FireTank = new CardType("Огненный танк", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(10)),
            SequentialEffectMakerItem.TriggerPlayEffect(false, x => x.Player.Played == null, x => new AttackEffect(15)),
            SequentialEffectMakerItem.End()
        ));

        public static CardType RepairBot = new CardType("Ремонтный бот", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.PlayEffect(true, x => new HealEffect(10)),
            SequentialEffectMakerItem.PlayEffect(true, x => new BlockEffect(5)),
            SequentialEffectMakerItem.TriggerPlayEffect(true, x => x.Player.Played == null, x => new HealEffect(10)),
            SequentialEffectMakerItem.PlayEffect(true, x => new BlockEffect(5)),
            SequentialEffectMakerItem.End()
        ));

        public static CardType HomingChicken = new CardType("Самонаводящаяся курица", Rarity.Rare, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.PlayEffect(true, x => new DiscardEffect()),
            SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(15)),
            SequentialEffectMakerItem.TriggerPlayEffect(false, x => x.Player.Played == null, x => new AttackEffect(20)),
            SequentialEffectMakerItem.End()
        ));

        public static CardType Thunder = new CardType("Гроза", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(15)),
            SequentialEffectMakerItem.PlayEffect(true, x => new RevealMatchingEffect(Zone.Hand, y => y.Type == Thunder)),
            SequentialEffectMakerItem.ConditionalPlayEffect(false, x => x.PrevEffects.EffectsFromCard(x.Card)
                .Any(y => y.Effect is RevealEffect && ((RevealEffect)y.Effect).Revealed != null), x => new AttackEffect(5)),
            SequentialEffectMakerItem.End()
        ));

        public static CardType Fireball = new CardType("Огненный шар", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.PlayEffect(true, x => new DiscardEffect()),
            SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(15)),
            SequentialEffectMakerItem.ConditionalPlayEffect(false, x => x.PrevEffects.EffectsFromCard(x.Card)
                .Any(y => y.Effect is DiscardEffect && ((DiscardEffect)y.Effect).Discarded != null), x => new AttackEffect(10)),
            SequentialEffectMakerItem.End()
        ));

        public static CardType FlameRitual = new CardType("Ритуал сожжения", Rarity.Rare, () => new SequentialEffectMaker(
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

        public static CardType Permafrost = new CardType("Вечная мерзлота", Rarity.Rare, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.PlayEffect(true, x => new BlockEffect(100)),
            SequentialEffectMakerItem.PlayEffect(false, x => new BlockEffect(100)),
            SequentialEffectMakerItem.End()
        ));

        public static CardType CurseOfFear = new CardType("Заклятие ужаса", Rarity.Common, () => new CurseOfFearEffectMaker());

        public static CardType SpikeTrap = new CardType("Ловушка с шипами", Rarity.Common, () => new SequentialEffectMaker(
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

        public static CardType CircularSaw = new CardType("Циркулярная пила", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            new SequentialEffectMakerItem(true,
                x => x.PrevEffects.Any(y => y.Effect is DiscardEffect && ((DiscardEffect)y.Effect).Discarded == x.Card),
                x => x.Card.Zone == Zone.Play, x => new MoveEffect(x.Card.Zone, Zone.Play)),
            SequentialEffectMakerItem.PlayEffect(false, x => new AttackEffect(15)),
            SequentialEffectMakerItem.End()
        ));

        public static CardType Clone = new CardType("Призыв клона", Rarity.Common, () => new CloneEffectMaker());

        public static CardType Thunderstorm = new CardType("Шторм", Rarity.Rare, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.PlayEffect(true, x => new DrawEffect()),
            SequentialEffectMakerItem.NextTurn(),
            SequentialEffectMakerItem.PlayEffect(true, x => new MoveMatchingEffect(Zone.Hand, Zone.Play, y => y.Type == Spark)),
            SequentialEffectMakerItem.NextTurn(),
            SequentialEffectMakerItem.PlayEffect(true, x => new MoveMatchingEffect(Zone.Hand, Zone.Play, y => y.Type == Spark)),
            SequentialEffectMakerItem.End()
        ));

        public static CardType Scout = new CardType("Разведка", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.PlayEffect(true, x => new BlockEffect(15)),
            SequentialEffectMakerItem.ConditionalPlayEffect(true, x => x.PrevEffects.CountAttackTaken(x.Player) > 0, x => new DrawEffect()),
            SequentialEffectMakerItem.End()
        ));

        public static CardType FirstAid = new CardType("Первая помощь", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.PlayEffect(true, x => new HealEffect(5)),
            SequentialEffectMakerItem.PlayEffect(true, x => new BlockEffect(10)),
            SequentialEffectMakerItem.PlayEffect(true, x => new DrawMatchingEffect(y => y.Type.CallIsolated(x.Settings, x.Random).Any(z => z.Effect is HealEffect))),
            SequentialEffectMakerItem.End()
        ));

        public static CardType Combo = new CardType("Комбо", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.StartInDeck(),
            SequentialEffectMakerItem.PlayEffect(true, x => new DrawEffect()),
            SequentialEffectMakerItem.ConditionalPlayEffect(false, x => x.PrevEffects.EffectsFromCard(x.Card)
                .Any(y => y.Effect is DrawEffect && ((DrawEffect)y.Effect).Drawn != null &&
                ((DrawEffect)y.Effect).Drawn.Type.CallIsolated(x.Settings, x.Random).Any(z => z.Effect is AttackEffect)),
                x => new AttackEffect(15)),
            SequentialEffectMakerItem.End()
        ));

        public static CardType CardsOfFate = new CardType("Судбоносные карты", Rarity.Common, () => new SequentialEffectMaker(
            SequentialEffectMakerItem.Super(),
            SequentialEffectMakerItem.PlayEffect(true, x => new MoveAllEffect(Zone.Hand, Zone.Play)),
            SequentialEffectMakerItem.End()
        ), Tag.Super);

        public static CardType TrappedCorridor = new CardType("Коридор ловушек", Rarity.Common, () => new SequentialEffectMaker(
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
            Thunderstorm, Scout, Combo, FirstAid, CardsOfFate
        });
    }
}
