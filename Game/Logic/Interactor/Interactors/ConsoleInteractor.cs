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

        private readonly TextReader input;
        private readonly TextWriter output;

        private Game game;
        private PlayerState player;
        private PlayerState opponent;

        private bool dispayedPlayedCard = false;

        public ConsoleInteractor(TextReader input, TextWriter output)
        {
            this.input = input;
            this.output = output;
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
                output.WriteLine("Ничья!");
            else if (player.Dead)
                output.WriteLine("Вы проиграли!");
            else
                output.WriteLine("Вы выиграли!");
            input.ReadLine();
            game = null;
            player = null;
            opponent = null;
        }

        public void AskForChoice()
        {
            dispayedPlayedCard = false;
            output.WriteLine();
            output.WriteLine($"Ваше здоровье: {player.Health}/{player.MaxHealth}");
            output.WriteLine($"Ваша зона игры: {Join(player.Play.Select(CardToString), ", ")}");
            output.WriteLine($"Ваша рука: {Join(player.Hand.Select(CardToString), ", ")}");
            output.WriteLine($"Ваша колода: {player.Deck.Count} карт, Ваш сброс: {player.Pile.Count} карт");
            output.WriteLine();
            output.WriteLine($"Здоровье противника: {opponent.Health}/{opponent.MaxHealth}");
            output.WriteLine($"Зона игры противника: {Join(opponent.Play.Select(x => x.Hidden ? "???" : CardToString(x)), ", ")}");
            output.WriteLine($"Рука противника: {opponent.Hand.Count} карт");
            output.WriteLine($"Колода противника: {opponent.Deck.Count} карт, сброс противника: {opponent.Pile.Count} карт");
            output.WriteLine();
            output.WriteLine("Сделайте ход:");
            output.WriteLine("(0) Пропуск хода");
            for (int i = 0; i < player.Hand.Count; ++i)
                output.WriteLine($"({i + 1}) {CardToStringDetailed(player.Hand[i])}");

            int result;
            while (true)
            {
                string s = input.ReadLine();
                if (int.TryParse(s, out result) && 0 <= result && result <= player.Hand.Count)
                    break;
            }
            output.WriteLine();
            output.WriteLine();
            MakeChoice(this, result == 0 ? null : player.Hand[result - 1]);
        }

        public event Game.ChioceEventHandler MakeChoice = new Game.ChioceEventHandler((x, y) => { });

        private string EffectCallToString(EffectCall effectCall)
        {
            string source, target;
            if (effectCall.Caller == null)
                source = "Глобальный эффект";
            else if (effectCall.CallerCard == null)
                source = effectCall.Caller == player ? "Ваш глобальный эффект" : "Глобальный эффект противника";
            else
                source = effectCall.Caller == player ? $"Эффект Вашей карты {CardToString(effectCall.CallerCard)}" :
                    $"Эффект карты противника {CardToString(effectCall.CallerCard)}";
            target = effectCall.Target == player ? "Вас" : "противника";
            return String.Format("{0} действует на {1}", source, target);
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
                    output.WriteLine("Противник разыгрывает ???!");
                else
                    output.WriteLine($"Противник разыгрывает {CardToStringDetailed(opponent.Played)}!");
                dispayedPlayedCard = true;
            }
            var effectString = EffectToString(effect.Effect);
            if (effectString != null)
                output.WriteLine("{0}: {1}", EffectCallToString(effect), effectString);
        }
    }
}
