using System;
using System.Collections.Generic;
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

        private readonly Dictionary<Assembly, AssemblyDefinition> asms = new Dictionary<Assembly, AssemblyDefinition>();

        public AssemblyPool(IAssemblyChecker assemblyChecker)
        {
            this.assemblyChecker = assemblyChecker;
        }

        public void Add(Assembly assembly)
        {
            asms.Add(assembly,
                     AssemblyDefinition.ReadAssembly(assembly.GetAssemblyPath(),
                                                     new ReaderParameters(ReadingMode.Immediate)
                                                         {
                                                             ReadSymbols = true
                                                         }));
        }

        public AssemblyDefinition GetAssemblyDefinition(Assembly assembly)
        {
            return asms[assembly];
        }

        public void Save(ErrorHandler errorHandler, Func<string, string> newAssemblyNameProvider)
        {
            foreach (var def in asms)
            {
                WeaveOneAssembly(def.Key.GetAssemblyPath(), def.Value, errorHandler, newAssemblyNameProvider);
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