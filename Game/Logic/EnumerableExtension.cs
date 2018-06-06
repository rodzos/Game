using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic
{
    public static class EnumerableExtension
    {
        public static T MaxBy<T>(this IEnumerable<T> sequence, Func<T, IComparable> key)
        {
            if (sequence == null)
                throw new ArgumentNullException();
            T result = default(T);
            IComparable resultKey = null;
            bool hasResult = false;
            foreach (var e in sequence)
            {
                var currentKey = key(e);
                if (!hasResult || resultKey.CompareTo(currentKey) < 0)
                {
                    result = e;
                    resultKey = currentKey;
                    hasResult = true;
                }
            }
            if (!hasResult)
                throw new ArgumentException();
            return result;
        }

        public static IEnumerable<TResult> SelectManyAlternate<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
        {
            var enums = source.Select(x => selector(x).GetEnumerator()).ToList();
            var ended = enums.Select(x => false).ToList();
            while (ended.Any(x => !x))
            {
                for (var i = 0; i < enums.Count; ++i)
                {
                    if (!ended[i] && enums[i].MoveNext())
                        yield return enums[i].Current;
                    else
                        ended[i] = true;
                }
            }
        }

        public static bool SetEqual<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TSource, bool> comparer)
        {
            List<TSource> firstList = first.ToList();
            List<TSource> secondList = second.ToList();
            foreach (var x in firstList)
                if (!secondList.Any(y => comparer(x, y)))
                    return false;
            foreach (var x in secondList)
                if (!firstList.Any(y => comparer(x, y)))
                    return false;
            return true;
        }
    }
}
