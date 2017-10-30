using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
