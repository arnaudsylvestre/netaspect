using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Helpers;

namespace FluentAspect.Weaver.Core.Model
{
    public class NetAspectDefinition
    {
        private object _attribute;

        public NetAspectDefinition(object attribute)
        {
            _attribute = attribute;
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

        public MethodInfo SelectorConstructor
        {
            get { return _attribute.GetMethod("WeaveConstructor"); }
        }

        public MethodInfo SelectorMethod
        {
            get { return _attribute.GetMethod("WeaveMethod"); }
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
            get
            {
                return Type.GetField("NetAspectAttribute", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static |
                                     BindingFlags.Instance) != null;
            }
        }

        public Interceptor AfterPropertyGet
        {
            get { return new Interceptor(_attribute.GetMethod("AfterPropertyGet")); }
        }

        public Interceptor BeforePropertyGet
        {
            get { return new Interceptor(_attribute.GetMethod("BeforePropertyGet")); }
        }

        public Interceptor AfterPropertySet
        {
            get { return new Interceptor(_attribute.GetMethod("AfterPropertySet")); }
        }

        public Interceptor BeforePropertySet
        {
            get { return new Interceptor(_attribute.GetMethod("BeforePropertySet")); }
        }

        public Interceptor AfterParameter
        {
           get { return new Interceptor(_attribute.GetMethod("AfterParameter")); }
        }

        public Interceptor BeforeParameter
        {
           get { return new Interceptor(_attribute.GetMethod("BeforeParameter")); }
        }

        public Interceptor OnExceptionParameter
        {
           get { return new Interceptor(_attribute.GetMethod("OnExceptionParameter")); }
        }

        public Interceptor AfterCallEvent
        {
            get { return new Interceptor(_attribute.GetMethod("AfterCallEvent")); }
        }

        public Interceptor BeforeCallEvent
        {
            get { return new Interceptor(_attribute.GetMethod("BeforeCallEvent")); }
        }
    }
}