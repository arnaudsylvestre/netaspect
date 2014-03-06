using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAspect.Weaver.Core.Model.Adapters;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.Model
{
    public static class MethodExtensions
    {
        public static bool AreEqual(this IMethod m, MethodBase info)
        {
            IType declaringType_L = m.DeclaringType;
            Type type_L = info.DeclaringType;
            return m.Name == info.Name && declaringType_L.AreEqual(type_L) &&
                   ParametersEqual(m, info);
        }

        public static bool AreEqual(this IType declaringType_L, Type type_L)
        {
            return declaringType_L.FullName == type_L.FullName;
        }

        public static bool AreEqual(this EventReference m, EventInfo info)
        {
            return m.Name == info.Name && m.DeclaringType.FullName == info.DeclaringType.FullName;
        }

        public static bool AreEqual(this FieldReference m, EventInfo info)
        {
            return m.Name == info.Name && m.DeclaringType.FullName == info.DeclaringType.FullName;
        }

        public static bool AreEqual(this ParameterDefinition m, ParameterInfo info)
        {
            if (!AreEqual(new MethodDefinitionAdapter((MethodReference) m.Method), (MethodBase) info.Member))
                return false;
            return m.Name == info.Name;
        }

        public static bool AreEqual(this FieldReference m, FieldInfo info)
        {
            return m.Name == info.Name && m.DeclaringType.FullName == info.DeclaringType.FullName;
        }

        public static bool AreEqual(this PropertyReference m, PropertyInfo info)
        {
            return m.Name == info.Name && m.DeclaringType.FullName == info.DeclaringType.FullName;
        }

        private static bool ParametersEqual(IMethod method_P, MethodBase info_P)
        {
            List<IParameter> parameters_L = method_P.Parameters.ToList();
            ParameterInfo[] parameterInfos_L = info_P.GetParameters();
            if (parameters_L.Count != parameterInfos_L.Length)
                return false;
            for (int i = 0; i < parameters_L.Count; i++)
            {
                if (parameterInfos_L[i].ParameterType.FullName == null)
                {
                    if (parameters_L[i].Type.Name != parameterInfos_L[i].ParameterType.Name)
                        return false;
                }
                else
                {
                    if (parameters_L[i].Type.FullName != parameterInfos_L[i].ParameterType.FullName)
                        return false;
                }
                if (parameters_L[i].Name != parameterInfos_L[i].Name)
                    return false;
            }
            return true;
        }
    }
}