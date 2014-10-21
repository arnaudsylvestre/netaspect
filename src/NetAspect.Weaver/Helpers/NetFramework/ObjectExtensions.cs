using System;
using System.Reflection;

namespace NetAspect.Weaver.Helpers.NetFramework
{
   public static class ObjectExtensions
   {
      public const BindingFlags BINDING_FLAGS =
         BindingFlags.NonPublic | BindingFlags.Public |
         BindingFlags.Instance | BindingFlags.Static;

      public static T GetValueForField<T>(this Type o, string field, Func<T> defaultValueProvider)
      {
          try
          {
              return (T)o.GetField(field, BINDING_FLAGS).GetValue(null);
          }
          catch (Exception)
          {
              return defaultValueProvider();
          }
      }

      public static bool FieldExists(this Type o, string field)
      {
          return o.GetField(field, BINDING_FLAGS) != null;
      }
   }
}
