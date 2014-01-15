﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAspect.Weaver.Core.Model;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.Configuration
{
    public interface IAssemblyDefinitionProvider
    {
        AssemblyDefinition GetAssemblyDefinition(Assembly assembly);
    }        

    public class WeavingConfiguration
    {
       private readonly IAssemblyDefinitionProvider _assemblyDefinitionProvider;
       private List<MethodMatch> _methods;
       private List<MethodMatch> _constructors;
       private List<FieldMatch> _fields;

        public WeavingConfiguration(IAssemblyDefinitionProvider assemblyDefinitionProvider)
        {
            _assemblyDefinitionProvider = assemblyDefinitionProvider;
            _methods = new List<MethodMatch>();
            _constructors = new List<MethodMatch>();
           _fields = new List<FieldMatch>();
        }

        public IEnumerable<MethodMatch> Methods
        {
            get { return _methods; }
        }

        public IEnumerable<MethodMatch> Constructors
        {
            get { return _constructors; }
        }

       public List<FieldMatch> Fields
       {
          get { return _fields; }
       }

       public void AddMethod(Func<IMethod, bool> matcher, IEnumerable<Assembly> assemblies,
                              MethodWeavingConfiguration methodWeavingConfigurations,
                              CallWeavingConfiguration callWeavingConfigurations, List<string> errors)
        {
           Check(callWeavingConfigurations, errors);
           Check(methodWeavingConfigurations, errors);
           if (errors.Count > 0)
              return;
           _methods.Add(new MethodMatch()
           {
              AssembliesToScan = from a in assemblies select _assemblyDefinitionProvider.GetAssemblyDefinition(a),
              Matcher = matcher,
              CallWeavingInterceptors = callWeavingConfigurations,
              MethodWeavingInterceptors = methodWeavingConfigurations,
           });
        }

        public void AddField(Func<FieldReference, bool> matcher, IEnumerable<Assembly> assemblies,
                              CallWeavingConfiguration callWeavingConfigurations, List<string> errors)
        {
           Check(callWeavingConfigurations, errors);
           if (errors.Count > 0)
              return;
           _fields.Add(new FieldMatch()
           {
              AssembliesToScan = from a in assemblies select _assemblyDefinitionProvider.GetAssemblyDefinition(a),
              Matcher = matcher,
              CallWeavingInterceptors = callWeavingConfigurations,
           });
        }

        private void Check(MethodWeavingConfiguration methodWeavingConfigurations, List<string> errors)
        {
            if (methodWeavingConfigurations != null && methodWeavingConfigurations.Type.GetConstructor(new Type[0]) == null)
                errors.Add(string.Format("The type {0} must have a default constructor", methodWeavingConfigurations.Type.FullName));
        }

        private static void Check(CallWeavingConfiguration callWeavingConfigurations, List<string> errors)
        {
            if (callWeavingConfigurations != null && callWeavingConfigurations.Type.GetConstructor(new Type[0]) == null)
                errors.Add(string.Format("The type {0} must have a default constructor", callWeavingConfigurations.Type.FullName));
        }

        public void AddConstructor(Func<IMethod, bool> matcher, IEnumerable<Assembly> assemblies,
                              MethodWeavingConfiguration methodWeavingConfigurations,
                              CallWeavingConfiguration callWeavingConfigurations, List<string> errors)
        {
            Check(callWeavingConfigurations, errors);
            Check(methodWeavingConfigurations, errors);
            if (errors.Count > 0)
                return;
            _constructors.Add(new MethodMatch()
            {
                AssembliesToScan = from a in assemblies select _assemblyDefinitionProvider.GetAssemblyDefinition(a),
                Matcher = matcher,
                CallWeavingInterceptors = callWeavingConfigurations,
                MethodWeavingInterceptors = methodWeavingConfigurations,
            });
        }
    }
}