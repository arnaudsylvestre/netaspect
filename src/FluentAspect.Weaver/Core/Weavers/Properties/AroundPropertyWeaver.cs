using System;
using System.Collections.Generic;
using FluentAspect.Weaver.Helpers;
using FluentAspect.Weaver.Weavers.Methods;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.Weavers.Properties
{
    public class AroundPropertyWeaver : IWeaveable
    {
        private readonly List<Type> _interceptorTypes;
        private readonly PropertyDefinition _propertyDefinition;

        public AroundPropertyWeaver(List<Type> interceptorTypes, PropertyDefinition propertyDefinition)
        {
            _interceptorTypes = interceptorTypes;
            _propertyDefinition = propertyDefinition;
        }

        public void Weave()
        {
            AroundMethodWeaver.WeaveMethod(_propertyDefinition.GetMethod, _interceptorTypes);
        }
    }
}