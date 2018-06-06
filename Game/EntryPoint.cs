﻿using Game.Logic;
using Game.Logic.EffectMakers;
using Game.Logic.Effects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Game.Logic.CardCollection;

namespace Game
{
    static class EntryPoint // This class doesn't really matter, hence all the bad code.
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
            foreach (var effect in game.MakeTurn(phase, () => new GlobalEffects()))
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

        public static List<bool> PlayInteractively(List<List<CardType>> decks)
        {
            var game = new GameState(decks);
            var random = new Random();
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
                    Console.WriteLine($"({i + 1}) {game.Players[0].Hand[i].Name}");

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
                        Console.WriteLine($"Игрок 2 разыгрывает {played.Name}!");
                }
                else
                    Console.WriteLine($"Игрок 2 пропускает свой ход!");

                InstantlyMakeTurn(game, Phase.Play, true);
                InstantlyMakeTurn(game, Phase.End, true);
                game.FinishTurn();
            }
            return game.Players.Select(x => x.Dead).ToList();
        }

        public static List<bool> Play(List<List<CardType>> decks)
        {
            if (decks.Count != 2)
                throw new ArgumentException();
            var random = new Random();
            GameState game = new GameState(decks);
            InstantlyMakeTurn(game, Phase.StartOfGame, false);
            while (game.Players.All(x => !x.Dead))
            {
                InstantlyMakeTurn(game, Phase.Choose, false);
                for (int i = 0; i < 2; ++i)
                    game.Players[i].MakeATurn(game.Players[i].Hand.Count > 0 ? random.Choice(game.Players[i].Hand) : null);
                InstantlyMakeTurn(game, Phase.Play, false);
                InstantlyMakeTurn(game, Phase.End, false);
                game.FinishTurn();
            }
            return game.Players.Select(x => x.Dead).ToList();
        }

        public static double Fitness(List<CardType> deck, List<CardType> otherDeck, int games)
        {
            var random = new Random();
            int score = 0;
            for (int i = 0; i < games; ++i)
            {
                var dead = Play(new List<List<CardType>> { deck, otherDeck });
                if (!dead[0])
                    score += 3;
                else if (dead[1])
                    score += 1;
            }
            return (double)score / games;
        }

        public static List<CardType> RandomDeck(Random random, int size)
        {
            var deck = new List<CardType>();
            while (deck.Count < size)
                deck.Add(random.Choice(AllCards.
                    Where(x => deck.Count(y => y == x) < (x.Rarity == Rarity.Common ? 2 : 1) &&
                          (!x.Tags.Contains(Tag.Auto) || deck.Count(y => y.Tags.Contains(Tag.Auto)) < 2) &&
                          (!x.Tags.Contains(Tag.Super) || deck.Count(y => y.Tags.Contains(Tag.Super)) < 1))
                ));
            return deck;
        }

        public static List<CardType> Nemesis(List<CardType> deck, int burnSteps, int gamesPerStep)
        {
            var random = new Random();
            var curDeck = RandomDeck(random, 15);
            var curFitness = Fitness(curDeck, deck, gamesPerStep);
            for (int t = burnSteps; t > 0; --t)
            {
                var nextDeck = curDeck.ToList();
                nextDeck.RemoveAt(random.Next(nextDeck.Count));
                nextDeck.Add(random.Choice(AllCards.
                    Where(x => nextDeck.Count(y => y == x) < (x.Rarity == Rarity.Common ? 2 : 1) &&
                          (!x.Tags.Contains(Tag.Auto) || nextDeck.Count(y => y.Tags.Contains(Tag.Auto)) < 2) &&
                          (!x.Tags.Contains(Tag.Super) || nextDeck.Count(y => y.Tags.Contains(Tag.Super)) < 1))
                ));
                var nextFitness = Fitness(nextDeck, deck, gamesPerStep);
                double threshold = Math.Exp((nextFitness - curFitness) * 5 / t * burnSteps);
                Console.WriteLine($"{t} {curFitness} {nextFitness} {threshold}");
                if (random.NextDouble() <= threshold)
                {
                    curDeck = nextDeck;
                    curFitness = nextFitness;
                }
            }
            return curDeck;
        }

        public static void Main()
        {
            var random = new Random();
            var deck = RandomDeck(random, 18);
            int diff;
            for (diff = 1; diff <= 10; ++diff)
            {
                deck = deck.OrderBy(x => x.Name).ToList();
                while (true)
                {
                    Console.WriteLine("Твоя колода: ");
                    for (int i = 0; i < deck.Count; ++i)
                        Console.WriteLine($"({i + 1}): {deck[i].Name}");
                    if (deck.Count == 15)
                        break;
                    Console.WriteLine("Убрать что-то?");
                    int removed;
                    while (true)
                    {
                        string s = Console.ReadLine();
                        if (int.TryParse(s, out removed) && 0 <= removed && removed <= deck.Count)
                            break;
                    }
                    if (removed > 0)
                        deck.RemoveAt(removed - 1);
                    else if (deck.Count <= 20)
                        break;
                }
                Console.ReadLine();
                var decks = new List<List<CardType>> { deck, Nemesis(deck, diff * 5, diff * 50) };
                var dead = PlayInteractively(decks);
                if (dead[0] && dead[1])
                {
                    Console.WriteLine("Ничья!");
                    Console.ReadLine();
                    --diff;
                    continue;
                }
                else if (dead[0])
                {
                    Console.WriteLine("Ты проиграл!");
                    Console.ReadLine();
                    --diff;
                    break;
                }
                Console.WriteLine("Ты выиграл!");
                if (diff == 10)
                    break;
                Console.ReadLine();
                Console.WriteLine("Награда:");
                for (int i = 0; i < 3; ++i)
                {
                    deck.Add(random.Choice(AllCards.
                           Where(x => deck.Count(y => y == x) < (x.Rarity == Rarity.Common ? 2 : 1) &&
                               (!x.Tags.Contains(Tag.Auto) || deck.Count(y => y.Tags.Contains(Tag.Auto)) < 2) &&
                               (!x.Tags.Contains(Tag.Super) || deck.Count(y => y.Tags.Contains(Tag.Super)) < 1))
                    ));
                    Console.WriteLine(deck.Last().Name);
                }
                Console.ReadLine();
            }
            Console.WriteLine("Игра окончена!");
            Console.WriteLine($"Твой результат: {diff}");
            Console.WriteLine($"Твоя колода: {Join(deck.Select(x => x.Name), ", ")}");
        }
    }
}