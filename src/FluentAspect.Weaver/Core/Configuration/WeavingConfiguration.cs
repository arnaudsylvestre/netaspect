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
       private List<AspectMatch<IMethod>> _methods;
       private List<AspectMatch<IMethod>> _constructors;
       private List<AspectMatch<FieldReference>> _fields;
       private List<AspectMatch<PropertyReference>> _properties;
       private List<AspectMatch<ParameterDefinition>> _parameters;
       private List<AspectMatch<FieldReference>> _events;

        public WeavingConfiguration(IAssemblyDefinitionProvider assemblyDefinitionProvider)
        {
            _assemblyDefinitionProvider = assemblyDefinitionProvider;
            _methods = new List<AspectMatch<IMethod>>();
            _constructors = new List<AspectMatch<IMethod>>();
           _fields = new List<AspectMatch<FieldReference>>();
           _properties = new List<AspectMatch<PropertyReference>>();
           _parameters = new List<AspectMatch<ParameterDefinition>>();
           _events = new List<AspectMatch<FieldReference>>();
        }

        public IEnumerable<AspectMatch<ParameterDefinition>> Parameters
        {
            get { return _parameters; }
        }

        public IEnumerable<AspectMatch<PropertyReference>> Properties
        {
            get { return _properties; }
        }

        public IEnumerable<AspectMatch<IMethod>> Methods
        {
            get { return _methods; }
        }

        public IEnumerable<AspectMatch<IMethod>> Constructors
        {
            get { return _constructors; }
        }

        public List<AspectMatch<FieldReference>> Fields
       {
          get { return _fields; }
       }

        public List<AspectMatch<FieldReference>> Events
        {
            get { return _events; }
        }

        public void AddMethod(Func<IMethod, bool> matcher, IEnumerable<Assembly> assemblies,
                               NetAspectDefinition methodWeavingConfigurations, List<string> errors)
        {
            Check(methodWeavingConfigurations, errors);
            if (errors.Count > 0)
                return;
            _methods.Add(new AspectMatch<IMethod>()
            {
                AssembliesToScan = from a in assemblies select _assemblyDefinitionProvider.GetAssemblyDefinition(a),
                Matcher = matcher,
                Aspect = methodWeavingConfigurations,
            });
        }

        public void AddParameter(Func<ParameterDefinition, bool> matcher, IEnumerable<Assembly> assemblies,
                               NetAspectDefinition methodWeavingConfigurations, List<string> errors)
        {
            Check(methodWeavingConfigurations, errors);
            if (errors.Count > 0)
                return;
            _parameters.Add(new AspectMatch<ParameterDefinition>()
            {
                AssembliesToScan = from a in assemblies select _assemblyDefinitionProvider.GetAssemblyDefinition(a),
                Matcher = matcher,
                Aspect = methodWeavingConfigurations,
            });
        }

       public void AddField(Func<FieldReference, bool> matcher, IEnumerable<Assembly> assemblies,
                             NetAspectDefinition aspect, List<string> errors)
       {
           Check(aspect, errors);
           if (errors.Count > 0)
               return;
           _fields.Add(new AspectMatch<FieldReference>()
           {
               AssembliesToScan = from a in assemblies select _assemblyDefinitionProvider.GetAssemblyDefinition(a),
               Matcher = matcher,
               Aspect = aspect,
           });
       }

       public void AddProperty(Func<PropertyReference, bool> matcher, IEnumerable<Assembly> assemblies,
                             NetAspectDefinition aspect, List<string> errors)
       {
           Check(aspect, errors);
           if (errors.Count > 0)
               return;
           _properties.Add(new AspectMatch<PropertyReference>()
               {
               AssembliesToScan = from a in assemblies select _assemblyDefinitionProvider.GetAssemblyDefinition(a),
               Matcher = matcher,
               Aspect = aspect,
           });
       }

        private void Check(NetAspectDefinition methodWeavingConfigurations, List<string> errors)
        {
            if (methodWeavingConfigurations != null && methodWeavingConfigurations.Type.GetConstructor(new Type[0]) == null)
                errors.Add(string.Format("The type {0} must have a default constructor", methodWeavingConfigurations.Type.FullName));
        }

        public void AddConstructor(Func<IMethod, bool> matcher, IEnumerable<Assembly> assemblies,
                              NetAspectDefinition aspect, List<string> errors)
        {
            Check(aspect, errors);
            if (errors.Count > 0)
                return;
            _constructors.Add(new AspectMatch<IMethod>()
            {
                AssembliesToScan = from a in assemblies select _assemblyDefinitionProvider.GetAssemblyDefinition(a),
                Matcher = matcher,
                Aspect = aspect,
            });
        }

        public void AddEvent(Func<FieldReference, bool> matcher, IEnumerable<Assembly> assemblies,
                              NetAspectDefinition aspect, List<string> errors)
        {
            Check(aspect, errors);
            if (errors.Count > 0)
                return;
            _events.Add(new AspectMatch<FieldReference>()
            {
                AssembliesToScan = from a in assemblies select _assemblyDefinitionProvider.GetAssemblyDefinition(a),
                Matcher = matcher,
                Aspect = aspect,
            });
        }
    }
}