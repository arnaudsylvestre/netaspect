using System.Collections.Generic;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Helpers;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.Weavers.Constructors
{
    public class AroundConstructorWeaver : IWeaveable
    {
        private readonly MethodDefinition definition;
        private readonly List<NetAspectAttribute> interceptorType;

        public AroundConstructorWeaver(List<NetAspectAttribute> interceptorType, MethodDefinition definition_P)
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
                                                                     List<NetAspectAttribute> interceptor)
        {
            MethodDefinition wrappedMethod = methodDefinition.Clone("-Weaved-Constructor");


            var weaver = new ConstructorAroundWeaver();
            weaver.CreateWeaver(methodDefinition, interceptor, wrappedMethod);
            methodDefinition.Body.InitLocals = true;
            return wrappedMethod;
        }
    }
}