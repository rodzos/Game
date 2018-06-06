using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic
{
    public static class RandomExtensions
    {
        public static T Choice<T>(this Random random, IEnumerable<T> sequence)
        {
            if (sequence == null)
                throw new ArgumentNullException();
            var chosen = default(T);
            bool any = false;
            int count = 0;
            foreach (var e in sequence)
            {
                any = true;
                ++count;
                if (random.Next(count) == 0)
                    chosen = e;
            }
            if (!any)
                throw new InvalidOperationException();
            return chosen;
        }

        public static T ChoiceOrDefault<T>(this Random random, IEnumerable<T> sequence)
        {
            var chosen = default(T);
            int count = 0;
            foreach (var e in sequence)
            {
                ++count;
                if (random.Next(count) == 0)
                    chosen = e;
            }
            return chosen;
        }

        public static IEnumerable<T> Shuffle<T>(this Random random, IEnumerable<T> sequence)
        {
            if (sequence == null)
                throw new ArgumentNullException();
            var list = new List<T>();
            foreach (var e in sequence)
                list.Insert(random.Next(list.Count + 1), e);
            return list;
        }

        public static void ShuffleInPlace<T>(this Random random, List<T> list)
        {
            if (list == null)
                throw new ArgumentNullException();
            for (int i = 1; i < list.Count; ++i)
            {
                var pos = random.Next(i + 1);
                var cur = list[i];
                for (int j = i; j > pos; --j)
                    list[j] = list[j - 1];
                list[pos] = cur;
            }
        }
    }
}
