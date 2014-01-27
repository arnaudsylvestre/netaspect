using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAspect.Weaver.Helpers
{
    public static class TypeExtensions
    {
        public static List<MethodInfo> GetAllMethods(this IEnumerable<Type> types, Func<MethodInfo, bool> filter)
        {
            var methods = new List<MethodInfo>();

            foreach (Type type in types)
            {
                methods.AddRange(
                    from m in
                        type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static |
                                        BindingFlags.Instance)
                    where filter(m)
                    select m);
            }

            return methods;
        }
        public static List<ParameterInfo> GetAllParameters(this IEnumerable<Type> types, Func<ParameterInfo, bool> filter)
        {
            var parameters = new List<ParameterInfo>();

            foreach (var methodInfo in GetAllMethods(types, info => true))
            {
                parameters.AddRange(
                    from m in
                        methodInfo.GetParameters()
                    where filter(m)
                    select m);
            }

            return parameters;
        }
       public static List<FieldInfo> GetAllFields(this IEnumerable<Type> types, Func<FieldInfo, bool> filter)
       {
           var methods = new List<FieldInfo>();

           foreach (Type type in types)
           {
               methods.AddRange(
                   from m in
                       type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static |
                                       BindingFlags.Instance)
                   where filter(m)
                   select m);
           }

           return methods;
       }
       public static List<PropertyInfo> GetAllProperties(this IEnumerable<Type> types, Func<PropertyInfo, bool> filter)
       {
           var methods = new List<PropertyInfo>();

           foreach (Type type in types)
           {
               methods.AddRange(
                   from m in
                       type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static |
                                       BindingFlags.Instance)
                   where filter(m)
                   select m);
           }

           return methods;
       }

        public static List<ConstructorInfo> GetAllConstructors(this IEnumerable<Type> types,
                                                               Func<ConstructorInfo, bool> filter)
        {
            var methods = new List<ConstructorInfo>();

            foreach (Type type in types)
            {
                methods.AddRange(
                    from m in
                        type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static |
                                             BindingFlags.Instance)
                    where filter(m)
                    select m);
            }

            return methods;
        }
    }
}