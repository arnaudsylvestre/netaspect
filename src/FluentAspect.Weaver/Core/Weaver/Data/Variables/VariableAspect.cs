using System;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Errors;
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

        public void Check(MethodDefinition method, ErrorHandler errorHandler, Type aspectType)
        {
            if (method.IsStatic && lifeCycle == LifeCycle.PerInstance)
                errorHandler.OnError(ErrorCode.ImpossibleToHavePerInstanceLifeCycleInStaticMethod, FileLocation.None, aspectType.FullName, method.Name, method.DeclaringType.FullName.Replace("/", "+"));
        }
    }
}