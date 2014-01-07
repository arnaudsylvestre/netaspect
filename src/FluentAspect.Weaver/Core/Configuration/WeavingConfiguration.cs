using System;
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

        public WeavingConfiguration(IAssemblyDefinitionProvider assemblyDefinitionProvider)
        {
            _assemblyDefinitionProvider = assemblyDefinitionProvider;
            _methods = new List<MethodMatch>();
            _constructors = new List<MethodMatch>();
        }

        public IEnumerable<MethodMatch> Methods
        {
            get { return _methods; }
        }

        public IEnumerable<MethodMatch> Constructors
        {
            get { return _constructors; }
        }

        public void AddMethod(Func<IMethod, bool> matcher, IEnumerable<Assembly> assemblies,
                              IEnumerable<MethodWeavingConfiguration> methodWeavingConfigurations,
                              IEnumerable<CallWeavingConfiguration> callWeavingConfigurations)
        {
            _methods.Add(new MethodMatch()
            {
                AssembliesToScan = from a in assemblies select _assemblyDefinitionProvider.GetAssemblyDefinition(a),
                Matcher = matcher,
                CallWeavingInterceptors = callWeavingConfigurations,
                MethodWeavingInterceptors = methodWeavingConfigurations,
            });
        }

        public void AddConstructor(Func<IMethod, bool> matcher, IEnumerable<Assembly> assemblies,
                              IEnumerable<MethodWeavingConfiguration> methodWeavingConfigurations,
                              IEnumerable<CallWeavingConfiguration> callWeavingConfigurations)
        {
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