using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Apis.AssemblyChecker;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Helpers;

namespace NetAspect.Weaver.Core.Assemblies
{
    public class AssemblyPool
    {
        private IAssemblyChecker assemblyChecker;
       private Func<TypeDefinition, bool> typesToSave;

        private readonly Dictionary<Assembly, AssemblyDefinition> asms = new Dictionary<Assembly, AssemblyDefinition>();

        public AssemblyPool(IAssemblyChecker assemblyChecker, Func<TypeDefinition, bool> typesToSave_P)
        {
           this.assemblyChecker = assemblyChecker;
           typesToSave = typesToSave_P;
        }

       public void Add(IEnumerable<Assembly> assemblies)
        {
            foreach (var assembly_L in assemblies)
            {
                asms.Add(assembly_L,
                         AssemblyDefinition.ReadAssembly(assembly_L.GetAssemblyPath(),
                                                         new ReaderParameters(ReadingMode.Immediate)
                                                         {
                                                             ReadSymbols = true
                                                         }));
                
            }
        }

        public AssemblyDefinition GetAssemblyDefinition(Assembly assembly)
        {
            return asms[assembly];
        }

        public void Save(ErrorHandler errorHandler, Func<string, string> newAssemblyNameProvider)
        {
            foreach (var def in asms)
            {
               var assemblyDefinition_L = def.Value;
               Clean(assemblyDefinition_L);
               WeaveOneAssembly(def.Key.GetAssemblyPath(), assemblyDefinition_L, errorHandler, newAssemblyNameProvider);
            }
        }

        private void Clean(AssemblyDefinition assemblyDefinition_L)
        {
           if (typesToSave == null) return;
           foreach (var type in assemblyDefinition_L.MainModule.GetTypes().Where(typeDefinition_L => !typesToSave(typeDefinition_L)).ToList())
           {
              assemblyDefinition_L.MainModule.Types.Remove(type);
           }
        }


       private void WeaveOneAssembly(string getAssemblyPath, AssemblyDefinition assemblyDefinition,
                                            ErrorHandler errorHandler, Func<string, string> newAssemblyNameProvider)
        {
            string targetFileName = newAssemblyNameProvider(getAssemblyPath);
            assemblyDefinition.Write(targetFileName, new WriterParameters
                {
                    WriteSymbols = true,
                });
            assemblyChecker.Check(targetFileName, errorHandler);
        }
    }
}