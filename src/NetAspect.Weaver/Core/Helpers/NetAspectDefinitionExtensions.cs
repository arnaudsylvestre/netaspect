using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NetAspect.Weaver.Core.Model.Aspect;

namespace NetAspect.Weaver.Core.Helpers
{
   public static class NetAspectDefinitionExtensions
   {
       static readonly List<Func<NetAspectDefinition, bool>> isMethodAspect = CreateForMethod();
       static readonly List<Func<NetAspectDefinition, bool>> isInstructionAspect = CreateForInstruction();

       public static bool IsMethodWeavingAspect(this NetAspectDefinition aspect)
       {
           return isMethodAspect.Any(func => func(aspect));
       }

       public static bool IsInstructionWeavingattribute(this NetAspectDefinition aspect)
       {
           return isInstructionAspect.Any(func => func(aspect));
       }

       private static List<Func<NetAspectDefinition, bool>> CreateForInstruction()
       {
           var isInstructionAspect = new List<Func<NetAspectDefinition, bool>>
               {
                   a => a.AfterCallConstructor.Methods.Any(),
                   a => a.AfterCallMethod.Methods.Any(),
                   a => a.AfterGetField.Methods.Any(),
                   a => a.AfterGetProperty.Methods.Any(),
                   a => a.AfterSetProperty.Methods.Any(),
                   a => a.AfterUpdateField.Methods.Any(),
                   a => a.BeforeCallConstructor.Methods.Any(),
                   a => a.BeforeCallMethod.Methods.Any(),
                   a => a.BeforeGetField.Methods.Any(),
                   a => a.BeforeGetProperty.Methods.Any(),
                   a => a.BeforeSetProperty.Methods.Any(),
                   a => a.BeforeUpdateField.Methods.Any(),
                   
               };
           return isInstructionAspect;
       }

       private static List<Func<NetAspectDefinition, bool>> CreateForMethod()
       {
           var isMethodAspect = new List<Func<NetAspectDefinition, bool>>
               {
                   a => a.AfterConstructor.Methods.Any(),
                   a => a.AfterConstructorForParameter.Methods.Any(),
                   a => a.AfterMethod.Methods.Any(),
                   a => a.AfterMethodForParameter.Methods.Any(),
                   a => a.AfterPropertyGetMethod.Methods.Any(),
                   a => a.AfterPropertySetMethod.Methods.Any(),
                   a => a.BeforeConstructor.Methods.Any(),
                   a => a.BeforeConstructorForParameter.Methods.Any(),
                   a => a.BeforeMethod.Methods.Any(),
                   a => a.BeforeMethodForParameter.Methods.Any(),
                   a => a.BeforePropertyGetMethod.Methods.Any(),
                   a => a.BeforePropertySetMethod.Methods.Any(),
                   a => a.OnExceptionConstructor.Methods.Any(),
                   a => a.OnExceptionConstructorForParameter.Methods.Any(),
                   a => a.OnExceptionMethod.Methods.Any(),
                   a => a.OnExceptionMethodForParameter.Methods.Any(),
                   a => a.OnExceptionPropertyGetMethod.Methods.Any(),
                   a => a.OnExceptionPropertySetMethod.Methods.Any(),
                   a => a.OnFinallyConstructor.Methods.Any(),
                   a => a.OnFinallyConstructorForParameter.Methods.Any(),
                   a => a.OnFinallyMethod.Methods.Any(),
                   a => a.OnFinallyMethodForParameter.Methods.Any(),
                   a => a.OnFinallyPropertyGetMethod.Methods.Any(),
                   a => a.OnFinallyPropertySetMethod.Methods.Any()
               };
           return isMethodAspect;
       }

       public static IEnumerable<Assembly> GetAssembliesToWeave(this IEnumerable<NetAspectDefinition> aspects_P,
         Assembly defaultAssembly)
      {
         var assemblies_L = new HashSet<Assembly>();
         assemblies_L.Add(defaultAssembly);
         foreach (NetAspectDefinition aspect_L in aspects_P)
         {
            IEnumerable<Assembly> assembliesToWeave = aspect_L.AssembliesToWeave;
            foreach (Assembly assembly in assembliesToWeave)
            {
               assemblies_L.Add(assembly);
            }
         }
         return assemblies_L;
      }
   }
}
