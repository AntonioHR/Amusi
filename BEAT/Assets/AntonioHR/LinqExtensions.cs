using System;
using System.Collections.Generic;
using System.Linq;

namespace AntonioHR
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> SkipIndex<T>(this IEnumerable<T> source, int index)
        {
            int counter = 0;
            foreach (var item in source)
            {
                if (counter != index)
                    yield return item;

                counter++;
            }
        }

        public static TSource OnlyOrDefault<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            var results = source.Take(2).ToArray();
            return results.Length == 1 ? results[0] : default(TSource);
        }
    }
}
