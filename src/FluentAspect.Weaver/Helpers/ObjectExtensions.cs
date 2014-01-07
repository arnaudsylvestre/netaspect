using System;
using System.Reflection;
using FluentAspect.Weaver.Core.Model;

namespace FluentAspect.Weaver.Helpers
{
   public static class ObjectExtensions
   {
      public const BindingFlags BINDING_FLAGS =
            BindingFlags.NonPublic | BindingFlags.Public |
            BindingFlags.Instance | BindingFlags.Static;

      public static object GetValueForField(this object o, string field)
      {
         var field_L = o.GetType().GetField(field, BINDING_FLAGS);
         return field_L.GetValue(o);
      }

      public static T GetValueForField<T>(this object o, string field, Func<T> defaultValueProvider)
      {
         try
         {
            var field_L = o.GetType().GetField(field, BINDING_FLAGS);
            return (T)field_L.GetValue(o);
         }
         catch (Exception)
         {
            return defaultValueProvider();
         }
      }

      public static MethodInfo GetMethod(this object o, string methodName)
      {
         return o.GetType().GetMethod(methodName, BINDING_FLAGS);
      }
   }
}