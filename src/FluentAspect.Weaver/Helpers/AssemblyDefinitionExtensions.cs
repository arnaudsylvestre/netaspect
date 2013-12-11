using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core
{
   public static class AssemblyDefinitionExtensions
   {

       public static List<MethodDefinition> GetAllMethods(this AssemblyDefinition assemblyDefinition, Func<MethodDefinition, bool> filter)
       {
           var methods = new List<MethodDefinition>();

           foreach (var type in assemblyDefinition.Modules.SelectMany(module => module.Types))
           {
               methods.AddRange(from m in type.Methods where filter(m) select m);
           }

           return methods;
       }

       public static List<MethodDefinition> GetAllConstructors(this AssemblyDefinition assemblyDefinition, Func<MethodDefinition, bool> filter)
       {
           var methods = new List<MethodDefinition>();

           foreach (var type in assemblyDefinition.Modules.SelectMany(module => module.Types))
           {
               methods.AddRange(from m in type.Methods where filter(m) select m);
           }

           return methods;
       }

       public static List<MethodDefinition> GetAllMethods(this AssemblyDefinition assemblyDefinition)
       {
          return (from moduleDefinition in assemblyDefinition.Modules 
                     from typeDefinition in moduleDefinition.GetTypes() 
                        from methodDefinition in typeDefinition.Methods 
                  select methodDefinition).ToList();
       }
   }
}