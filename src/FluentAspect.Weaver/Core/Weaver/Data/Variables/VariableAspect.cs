using System;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Weaver.Engine.Lifecycle;

namespace NetAspect.Weaver.Core.Weaver.Data.Variables
{
    public class VariableAspect
    {

        private readonly AspectBuilder aspectBuilder;
        private LifeCycle lifeCycle;

        public VariableAspect(AspectBuilder aspectBuilder, LifeCycle lifeCycle)
        {
            this.aspectBuilder = aspectBuilder;
            this.lifeCycle = lifeCycle;
        }

        public VariableDefinition Build(InstructionsToInsert instructionsToInsert_P, MethodDefinition method, Type aspectType)
        {
            var interceptorVariable = new VariableDefinition(method.Module.Import(aspectType));
            aspectBuilder.CreateInterceptor(aspectType, lifeCycle, method, interceptorVariable, instructionsToInsert_P.aspectInitialisation);
            return interceptorVariable;
        }
    }
}