using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentAspect.Weaver.Core.Model
{
    public class MethodWeavingConfiguration
    {
        private readonly object _attribute;

        public MethodWeavingConfiguration(object attribute_P)
        {
            _attribute = attribute_P;
        }



        public MethodInfo SelectorMethod
        {
           get { return _attribute.GetType().GetMethod("WeaveMethod", NetAspectAttribute.BINDING_FLAGS); }
        }

        public IEnumerable<Assembly> AssembliesToWeave
        {
            get
            {
               FieldInfo fieldInfo_L = _attribute.GetType().GetField("AssembliesToWeave", NetAspectAttribute.BINDING_FLAGS);
                if (fieldInfo_L == null)
                    return new Assembly[0];
                return (IEnumerable<Assembly>)fieldInfo_L.GetValue(_attribute);
            }
        }

        public Type Type
        {
            get { return _attribute.GetType(); }
        }

        public Interceptor Before
        {
            get { return new Interceptor(_attribute.GetInterceptorMethod("Before")); }
        }

        public Interceptor After
        {
            get { return new Interceptor(_attribute.GetInterceptorMethod("After")); }
        }

        public Interceptor OnException
        {
            get { return new Interceptor(_attribute.GetInterceptorMethod("OnException")); }
        }
    }
}