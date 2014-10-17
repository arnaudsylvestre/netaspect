using System.Collections.Generic;
using Mono.Collections.Generic;

namespace NetAspect.Weaver.Helpers.IL
{
   public static class CollectionExtensions
   {
      public static void AddRange<T>(this Collection<T> collection, IEnumerable<T> toAdd)
      {
         foreach (T item_L in toAdd)
         {
            collection.Add(item_L);
         }
      }
   }
}
