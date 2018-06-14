using Game.Logic;
using Game.Logic.EffectMakers;
using Game.Logic.Effects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Game.Logic.CardCollection;
using NUnitLite;

namespace Game
{
    static class EntryPoint // This class doesn't really matter, hence all the bad code. Will rewrite it.
    {
        public static string Comment(EffectCall effect)
        {
            string playerString = effect.Caller == null ? "" : effect.Caller == effect.Caller.Game.Players[0] ? "Игрок 1, " : "Игрок 2, ";
            string cardString = effect.CallerCard == null ? "Глобальный эффект " : "Карта " + effect.CallerCard.Name + " ";
            string targetString = effect.Target == effect.Target.Game.Players[0] ? "действует на игрока 1: " : "действует на игрока 2: ";
            string effectString = "Неизвестный эффект";

            if (effect.Effect is AttackEffect)
                effectString = $"Атака {((AttackEffect)effect.Effect).Value}";
            if (effect.Effect is HealEffect)
                effectString = $"Лечение {((HealEffect)effect.Effect).Value}";
            if (effect.Effect is BlockEffect)
                effectString = $"Блок {((BlockEffect)effect.Effect).Value}";
            if (effect.Effect is DrawEffect)
            {
                if (((DrawEffect)effect.Effect).Success)
                    effectString = $"Возьми карту";
                else if (((DrawEffect)effect.Effect).Drawn != null)
                    effectString = $"Возьми карту (не удалось взять карту {((DrawEffect)effect.Effect).Drawn.Name})";
                else
                    effectString = $"Возьми карту (не удалось взять карту)";
            }
            if (effect.Effect is DiscardEffect)
            {
                if (((DiscardEffect)effect.Effect).Discarded == null)
                    effectString = $"Сбрось карту (ничего не сброшено)";
                else
                    effectString = $"Сбрось карту (сброшено {((DiscardEffect)effect.Effect).Discarded.Name})";
            }
            if (effect.Effect is AutoEffect)
                return null;
            if (effect.Effect is EndEffect)
                return null;
            if (effect.Effect is FinishTurnEffect)
                return null;
            if (effect.Effect is LoseHealthEffect)
                effectString = $"Потеряй {((LoseHealthEffect)effect.Effect).Value} здоровья";
            if (effect.Effect is MoveEffect)
            {
                if (((MoveEffect)effect.Effect).Moved == null)
                    effectString = $"Перемести карту из {((MoveEffect)effect.Effect).From} в {((MoveEffect)effect.Effect).To} (ничего не перемещено)";
                else
                    effectString = $"Перемести карту из {((MoveEffect)effect.Effect).From} в {((MoveEffect)effect.Effect).To} (перемещена {((MoveEffect)effect.Effect).Moved.Name})";
            }
            if (effect.Effect is RevealEffect)
            {
                if (((RevealEffect)effect.Effect).Revealed == null)
                    effectString = $"Покажи карту из руки (ничего не показано)";
                else
                    effectString = $"Покажи карту из руки (показана {((RevealEffect)effect.Effect).Revealed.Name})";
            }
            if (effect.Effect is StartInDeckEffect)
                return null;
            if (effect.Effect is HideEffect)
                return null;
            if (effect.Effect is UnhideEffect)
            {
                if (effect.CallerCard == null)
                    return null;
                else
                    effectString = $"Открой скрытую карту (открыта {effect.CallerCard.Name})";
            }
            if (effect.Effect is LoseMaxHealthEffect)
                effectString = $"Потеряй {((LoseMaxHealthEffect)effect.Effect).Value} максимального здоровья";
            if (effect.Effect is MoveAllEffect)
            {
                if (((MoveAllEffect)effect.Effect).AllMoved.Count == 0)
                    effectString = $"Перемести карты из {((MoveAllEffect)effect.Effect).From} в {((MoveAllEffect)effect.Effect).To} (ничего не перемещено)";
                else
                    effectString = $"Перемести карты из {((MoveAllEffect)effect.Effect).From} в {((MoveAllEffect)effect.Effect).To} (перемещено {Join(((MoveAllEffect)effect.Effect).AllMoved.Select(x => x.Name), ", ")})";
            }

            return (playerString + cardString + targetString + effectString);
        }

        public static void InstantlyMakeTurn(GameState game, Phase phase, bool verbose)
        {
            foreach (var effect in game.MakeTurn(phase))
            {
                effect.Call();
                if (verbose)
                {
                    var comment = Comment(effect);
                    if (comment != null)
                        Console.WriteLine(comment);
                }
            }
        }

        public static string Join(IEnumerable<string> cards, string sep)
        {
            var builder = new StringBuilder();
            bool first = true;
            foreach (var s in cards)
            {
                if (!first)
                    builder.Append(sep);
                first = false;
                builder.Append(s);
            }
            return builder.ToString();
        }

        public static List<bool> PlayInteractively(List<Deck> decks, Random random = null)
        {
            var game = new GameState(decks, () => new GlobalEffects(), new GameRules(), random);
            InstantlyMakeTurn(game, Phase.StartOfGame, true);
            while (game.Players.All(x => !x.Dead))
            {
                InstantlyMakeTurn(game, Phase.Choose, true);
                if (game.Players.Any(x => x.Dead))
                    break;

                Console.WriteLine();
                Console.WriteLine($"Ваше здоровье: {game.Players[0].Health}/{game.Players[0].MaxHealth}");
                Console.WriteLine($"Ваша зона игры: {Join(game.Players[0].Play.Select(x => x.Name), ", ")}");
                Console.WriteLine($"Ваша рука: {Join(game.Players[0].Hand.Select(x => x.Name), ", ")}");
                Console.WriteLine($"Ваша колода: {game.Players[0].Deck.Count} карт, Ваш сброс: {game.Players[0].Pile.Count} карт");
                Console.WriteLine();
                Console.WriteLine($"Здоровье противника: {game.Players[1].Health}/{game.Players[1].MaxHealth}");
                Console.WriteLine($"Зона игры противника: {Join(game.Players[1].Play.Select(x => x.Hidden ? "???" : x.Name), ", ")}");
                Console.WriteLine($"Рука противника: {game.Players[1].Hand.Count} карт");
                Console.WriteLine($"Колода противника: {game.Players[1].Deck.Count} карт, сброс противника: {game.Players[1].Pile.Count} карт");
                Console.WriteLine();
                Console.WriteLine("Сделай ход, СЕЙЧАС!");
                Console.WriteLine("(0) Пропуск хода");
                for (int i = 0; i < game.Players[0].Hand.Count; ++i)
                    Console.WriteLine($"({i + 1}) {game.Players[0].Hand[i].Name} ({game.Players[0].Hand[i].Description})");

                int result;
                while (true)
                {
                    string s = Console.ReadLine();
                    if (int.TryParse(s, out result) && 0 <= result && result <= game.Players[0].Hand.Count)
                        break;
                }
                game.Players[0].MakeATurn(result == 0 ? null : game.Players[0].Hand[result - 1]);
                CardState played = game.Players[1].Hand.Count > 0 ? random.Choice(game.Players[1].Hand) : null;
                game.Players[1].MakeATurn(played);
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                if (played != null)
                {
                    if (played.Hidden)
                        Console.WriteLine($"Игрок 2 разыгрывает ???!");
                    else
                        Console.WriteLine($"Игрок 2 разыгрывает {played.Name}! ({played.Description})");
                }
                else
                    Console.WriteLine($"Игрок 2 пропускает свой ход!");

                InstantlyMakeTurn(game, Phase.Play, true);
                InstantlyMakeTurn(game, Phase.End, true);
            }
            return game.Players.Select(x => x.Dead).ToList();
        }

        public static void Main(string[] args)
        {
            //new AutoRun().Execute(args);
            var random = new Random();
            var rules = new GameRules();
            var decks = new List<Deck>();
            for (int i = 0; i < 2; ++i)
                decks.Add(Deck.MakeRandomDeck(AllCards, rules, random));
            var dead = PlayInteractively(decks, random);
            if (dead[0] && dead[1])
                Console.WriteLine("Ничья!");
            else if (dead[0])
                Console.WriteLine("Ты проиграл!");
            else
                Console.WriteLine("Ты выиграл!");
            Console.ReadLine();
        }
    }
}
