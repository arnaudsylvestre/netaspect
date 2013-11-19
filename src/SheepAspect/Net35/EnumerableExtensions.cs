using System.Collections.Generic;

namespace System.Collections.Generic
{
   public static class EnumerableExtensions
   {
       public static IEnumerable<T> AsParallel<T>(this IEnumerable<T> enumerable_P)
       {
          return enumerable_P;
       }
   }
}