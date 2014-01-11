using System.Collections.Generic;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Helpers;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Constructors
{
    public class AroundConstructorWeaver : IWeaveable
    {
        private readonly MethodDefinition definition;
        private readonly IEnumerable<MethodWeavingConfiguration> interceptorType;

        public AroundConstructorWeaver(IEnumerable<MethodWeavingConfiguration> interceptorType, MethodDefinition definition_P)
        {
            this.interceptorType = interceptorType;
            definition = definition_P;
        }

        public void Weave(ErrorHandler errorP_P)
        {
            MethodDefinition newMethod = CreateNewMethodBasedOnMethodToWeave(definition, interceptorType);
            definition.DeclaringType.Methods.Add(newMethod);
        }

        public void Check(ErrorHandler errorHandler)
        {
        }

        public bool CanWeave()
        {
            return true;
        }

        private MethodDefinition CreateNewMethodBasedOnMethodToWeave(MethodDefinition methodDefinition,
                                                                     IEnumerable<MethodWeavingConfiguration> interceptor)
        {
            MethodDefinition wrappedMethod = methodDefinition.Clone("-Weaved-Constructor");


            var weaver = new ConstructorAroundWeaver();
            weaver.CreateWeaver(methodDefinition, interceptor, wrappedMethod);
            methodDefinition.Body.InitLocals = true;
            return wrappedMethod;
        }
    }
}