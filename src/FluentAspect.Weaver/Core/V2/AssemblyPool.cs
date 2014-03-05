using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Helpers;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.V2
{
    public class AssemblyPool
    {
        Dictionary<Assembly, AssemblyDefinition> asms = new Dictionary<Assembly, AssemblyDefinition>();

        public void Add(Assembly assembly)
        {
            asms.Add(assembly, AssemblyDefinition.ReadAssembly(assembly.GetAssemblyPath(), new ReaderParameters(ReadingMode.Immediate)
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


        public static void WeaveOneAssembly(string getAssemblyPath, AssemblyDefinition assemblyDefinition, ErrorHandler errorHandler, Func<string, string> newAssemblyNameProvider)
        {
            string targetFileName = newAssemblyNameProvider(getAssemblyPath);
            assemblyDefinition.Write(targetFileName, new WriterParameters
                {
                    WriteSymbols = true,
                });
            CheckAssembly(targetFileName, errorHandler);
        }



        public static void CheckAssembly(string targetFileName, ErrorHandler errorHandler)
        {
            try
            {
                PEVerify.Run(targetFileName);
            }
            catch (Exception e)
            {
                errorHandler.Errors.Add("An internal error has occured : " + e.Message);
            }
        }
    }
}