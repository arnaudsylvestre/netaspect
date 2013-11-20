using System;
using System.Collections.Generic;

namespace FluentAspect.Weaver.Helpers
{
    public static class CollectionHelper
    {
        

        public static void TransferItemsTo<T>(this ICollection<T> source, ICollection<T> target)
        {
            foreach (var s in source)
                target.Add(s);
            
            source.Clear();
        }
    }
}