using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.Interactor.Interactors
{
    class ConsoleInteractor : IInteractor
    {
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

        public static string CardToString(CardState card)
        {
            return card.Name;
        }

        public static string CardToStringDetailed(CardState card)
        {
            return $"{card.Name} ({card.Description})";
        }

        private Game game;
        private PlayerState player;
        private PlayerState opponent;

        private bool dispayedPlayedCard = false;

        public ConsoleInteractor()
        {

        }

        public void StartGame(Game game, PlayerState player)
        {
            this.game = game;
            this.player = player;
            opponent = game.GameState.Opponent(player);
        }

        public void EndGame()
        {
            if (player.Dead && opponent.Dead)
                Console.WriteLine("Ничья!");
            else if (player.Dead)
                Console.WriteLine("Вы проиграли!");
            else
                Console.WriteLine("Вы выиграли!");
            Console.ReadLine();
            game = null;
            player = null;
            opponent = null;
        }

        public void AskForChoice()
        {
            dispayedPlayedCard = false;
            Console.WriteLine();
            Console.WriteLine($"Ваше здоровье: {player.Health}/{player.MaxHealth}");
            Console.WriteLine($"Ваша зона игры: {Join(player.Play.Select(CardToString), ", ")}");
            Console.WriteLine($"Ваша рука: {Join(player.Hand.Select(CardToString), ", ")}");
            Console.WriteLine($"Ваша колода: {player.Deck.Count} карт, Ваш сброс: {player.Pile.Count} карт");
            Console.WriteLine();
            Console.WriteLine($"Здоровье противника: {opponent.Health}/{opponent.MaxHealth}");
            Console.WriteLine($"Зона игры противника: {Join(opponent.Play.Select(x => x.Hidden ? "???" : CardToString(x)), ", ")}");
            Console.WriteLine($"Рука противника: {opponent.Hand.Count} карт");
            Console.WriteLine($"Колода противника: {opponent.Deck.Count} карт, сброс противника: {opponent.Pile.Count} карт");
            Console.WriteLine();
            Console.WriteLine("Сделайте ход:");
            Console.WriteLine("(0) Пропуск хода");
            for (int i = 0; i < player.Hand.Count; ++i)
                Console.WriteLine($"({i + 1}) {CardToStringDetailed(player.Hand[i])}");

            int result;
            while (true)
            {
                string s = Console.ReadLine();
                if (int.TryParse(s, out result) && 0 <= result && result <= player.Hand.Count)
                    break;
            }
            Console.WriteLine();
            Console.WriteLine();
            MakeChoice(this, result == 0 ? null : player.Hand[result - 1]);
        }

        public event Game.ChioceEventHandler MakeChoice = new Game.ChioceEventHandler((x, y) => { });

        private string EffectCallToString(EffectCall effectCall)
        {
            if (effectCall.Caller == null)
                return "Глобальный эффект";
            else if (effectCall.CallerCard == null)
                return effectCall.Caller == player ? "Ваш глобальный эффект" : "Глобальный эффект противника";
            else
                return effectCall.Caller == player ? $"Эффект Вашей карты {CardToString(effectCall.CallerCard)}" :
                    $"Эффект карты противника {CardToString(effectCall.CallerCard)}";
        }

        private string EffectToString(Effect effect)
        {
            foreach (var displayer in EffectDisplayer.AllDisplayers)
            {
                var type = effect.GetType();
                if (type == displayer.EffectType || type.IsSubclassOf(displayer.EffectType))
                    return displayer.Display(effect);
            }
            throw new ArgumentException("Неизвестный эффект");
        }

        public void DisplayEffect(EffectCall effect)
        {
            if (dispayedPlayedCard == false && opponent.Played != null)
            {
                if (opponent.Played.Hidden)
                    Console.WriteLine("Противник разыгрывает ???!");
                else
                    Console.WriteLine($"Противник разыгрывает {CardToStringDetailed(opponent.Played)}!");
                dispayedPlayedCard = true;
            }
            var effectString = EffectToString(effect.Effect);
            if (effectString != null)
                Console.WriteLine("{0}: {1}", EffectCallToString(effect), effectString);
        }
    }
}
