using System.Collections.Generic;
using System.Reflection;

namespace System.Threading.Tasks
{
   public class Parallel
   {
      public static void ForEach<TSource>(IEnumerable<TSource> collection, Action<TSource> action)
      {
         foreach (var e in collection)
         {
            action(e);
         }
      }
   }
}