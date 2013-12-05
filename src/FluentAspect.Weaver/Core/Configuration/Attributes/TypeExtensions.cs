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
                methods.AddRange(from m in type.GetMethods() where filter(m) select m);
            }

            return methods;
        }
         
    }
}