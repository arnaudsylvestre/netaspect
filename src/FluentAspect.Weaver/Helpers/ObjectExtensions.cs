﻿using System;
using System.Reflection;
using FluentAspect.Weaver.Core.Model;

namespace FluentAspect.Weaver.Helpers
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
   }
}