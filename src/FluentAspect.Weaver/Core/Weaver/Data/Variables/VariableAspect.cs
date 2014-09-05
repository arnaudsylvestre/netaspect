using System;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Weaver.Engine.Lifecycle;

namespace NetAspect.Weaver.Core.Weaver.Data.Variables
{
    public class VariableAspect : Variable.IVariableBuilder
    {

        private readonly AspectBuilder aspectBuilder;
        private Type aspectType;
        private LifeCycle lifeCycle;

        public VariableAspect(AspectBuilder aspectBuilder, Type aspectType, LifeCycle lifeCycle)
        {
            this.aspectBuilder = aspectBuilder;
            this.aspectType = aspectType;
            this.lifeCycle = lifeCycle;
        }

        public VariableDefinition Build(InstructionsToInsert instructionsToInsert_P, MethodDefinition method, Instruction instruction)
        {
            var interceptorVariable = new VariableDefinition(method.Module.Import(aspectType));
            aspectBuilder.CreateInterceptor(aspectType, lifeCycle, method, interceptorVariable, instructionsToInsert_P.aspectInitialisation);
            return interceptorVariable;
        }
    }
}