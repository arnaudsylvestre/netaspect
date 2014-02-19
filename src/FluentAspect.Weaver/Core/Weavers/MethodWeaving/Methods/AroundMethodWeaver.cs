﻿using System;
using System.Collections.Generic;
using System.Linq;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Model;
using FluentAspect.Weaver.Helpers.IL;
using Mono.Cecil;
using MethodAttributes = Mono.Cecil.MethodAttributes;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Methods
{
    public class AroundMethodWeaver : IWeaveable
    {
        private readonly MethodDefinition definition;
       private NewAroundMethodWeaver weaver;

       public AroundMethodWeaver(IEnumerable<MethodWeavingConfiguration> interceptorType, MethodDefinition definition_P, Func<MethodToWeave, IMethodWeaver> methodWeaverFactory)
        {
          definition = definition_P;

            var methodToWeave_L = new MethodToWeave(interceptorType.ToList(), new Method(definition));
            weaver = new NewAroundMethodWeaver(methodToWeave_L, methodWeaverFactory(methodToWeave_L));
        }

        public void Weave()
        {
            weaver.Weave();
        }

        public void Check(ErrorHandler errorHandler)
        {
            if (!definition.HasBody)
            {
                if (definition.DeclaringType.IsInterface)
                    errorHandler.Errors.Add(string.Format("A method declared in interface can not be weaved : {0}.{1}",
                                                      definition.DeclaringType.Name, definition.Name));
                else if ((definition.Attributes & MethodAttributes.Abstract) == MethodAttributes.Abstract)
                    errorHandler.Errors.Add(string.Format("An abstract method can not be weaved : {0}.{1}",
                                                      definition.DeclaringType.Name, definition.Name));
            }
            weaver.Check(errorHandler);
        }

        

    }
}