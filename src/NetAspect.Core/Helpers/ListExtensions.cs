using System;
using System.Collections.Generic;

namespace NetAspect.Core.Helpers
{

    public static class ListExtensions
    {
        public delegate bool ChunckStarter<T>(T element);

        public static List<T> Until<T>(this IEnumerable<T> source, ChunckStarter<T> startChunk)
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