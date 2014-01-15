using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Helpers;

namespace FluentAspect.Weaver.Core.Model
{
    public class CallWeavingConfiguration
    {
        private readonly object _attribute;

        public CallWeavingConfiguration(object attribute_P)
        {
            _attribute = attribute_P;
        }

        public Interceptor BeforeInterceptor
        {
           get { return new Interceptor(_attribute.GetMethod("BeforeCall")); }
        }

        public Interceptor AfterInterceptor
        {
           get { return new Interceptor(_attribute.GetMethod("AfterCall")); }
        }

        public Interceptor BeforeUpdateFieldValue
        {
           get { return new Interceptor(_attribute.GetMethod("BeforeUpdateFieldValue")); }
        }

        public Interceptor AfterUpdateFieldValue
        {
           get { return new Interceptor(_attribute.GetMethod("AfterUpdateFieldValue")); }
        }

       public bool IsValid
       {
          get { return _attribute.GetAspectKind() == NetAspectAttributeKind.CallWeaving; }
       }


       public IEnumerable<Assembly> AssembliesToWeave
       {
           get
           {
               return _attribute.GetValueForField<IEnumerable<Assembly>>("AssembliesToWeave", () => new Assembly[0]);
           }
       }

       public MethodInfo SelectorConstructor
       {
           get { return _attribute.GetMethod("WeaveConstructor"); }
       }

       public MethodInfo SelectorMethod
       {
           get { return _attribute.GetMethod("WeaveMethod"); }
       }

       public Type Type
       {
          get { return _attribute.GetType(); }
       }
    }
}