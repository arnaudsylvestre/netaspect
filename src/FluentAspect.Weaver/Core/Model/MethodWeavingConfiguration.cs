using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Helpers;

namespace FluentAspect.Weaver.Core.Model
{
    public interface IWeavingConfiguration
    {
        IEnumerable<Assembly> AssembliesToWeave { get; }
        Type Type { get; }
        MethodInfo SelectorMethod { get; }
        MethodInfo SelectorConstructor { get; }
    }

    public class MethodWeavingConfiguration
    {
       private readonly object _attribute;

       public MethodWeavingConfiguration(object attribute_P)
        {
            _attribute = attribute_P;
        }

        public IEnumerable<Assembly> AssembliesToWeave
        {
            get
            {
               return _attribute.GetValueForField<IEnumerable<Assembly>>("AssembliesToWeave", () => new Assembly[0]);
            }
        }

        public Type Type
        {
            get { return _attribute.GetType(); }
        }

        public Interceptor Before
        {
            get { return new Interceptor(_attribute.GetMethod("Before")); }
        }

        public Interceptor After
        {
            get { return new Interceptor(_attribute.GetMethod("After")); }
        }

        public Interceptor OnException
        {
            get { return new Interceptor(_attribute.GetMethod("OnException")); }
        }

       public bool IsValid
       {
          get { return _attribute.GetAspectKind() == NetAspectAttributeKind.MethodWeaving; }
       }

        public MethodInfo SelectorConstructor
       {
           get { return _attribute.GetMethod("WeaveConstructor"); }
       }

        public MethodInfo SelectorMethod
        {
            get { return _attribute.GetMethod("WeaveMethod"); }
        }
    }
}