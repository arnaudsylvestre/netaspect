using System;
using System.Collections.Generic;

namespace NetAspect.Core.Helpers
{

    public static class ListExtensions
    {
        public static List<T> Until<T>(this IEnumerable<T> source, Func<T, bool> startChunk)
        {
            var list = new List<T>();

            foreach (var item in source)
            {
                if (startChunk(item))
                {
                    return list;
                }

                list.Add(item);
            }
            return new List<T>();
        }
    }
}