using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAspect.Weaver.Core.Fluent
{
    public static class TypeExtensions
    {
        public static List<MethodInfo> GetAllMethods(this IEnumerable<Type> types, Func<MethodInfo, bool> filter)
        {
            var methods = new List<MethodInfo>();

            foreach (var type in types)
            {
                methods.AddRange(from m in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance) where filter(m) select m);
            }

            return methods;
        }
        public static List<ConstructorInfo> GetAllConstructors(this IEnumerable<Type> types, Func<ConstructorInfo, bool> filter)
        {
            var methods = new List<ConstructorInfo>();

            foreach (var type in types)
            {
                methods.AddRange(from m in type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance) where filter(m) select m);
            }

            return methods;
        }
        public static List<PropertyInfo> GetAllProperties(this IEnumerable<Type> types, Func<PropertyInfo, bool> filter)
        {
            var methods = new List<PropertyInfo>();

            foreach (var type in types)
            {
                methods.AddRange(from m in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance) where filter(m) select m);
            }

            return methods;
        }
         
    }
}