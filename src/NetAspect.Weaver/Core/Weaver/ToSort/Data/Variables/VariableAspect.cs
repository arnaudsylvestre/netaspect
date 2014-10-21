using System;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Errors;
using NetAspect.Weaver.Core.Weaver.ToSort.Engine.LifeCycle;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables
{
    public class VariableAspect : Variable.IVariableBuilder
    {

        private readonly AspectInstanceBuilder _aspectInstanceBuilder;
        private LifeCycle lifeCycle;
        private Type aspectType;
        private CustomAttribute customAttribute;

        public VariableAspect(AspectInstanceBuilder _aspectInstanceBuilder, LifeCycle lifeCycle, Type aspectType, CustomAttribute customAttribute)
        {
            this._aspectInstanceBuilder = _aspectInstanceBuilder;
            this.lifeCycle = lifeCycle;
            this.aspectType = aspectType;
            this.customAttribute = customAttribute;
        }

        public VariableDefinition Build(InstructionsToInsert instructionsToInsert_P, MethodDefinition method, Mono.Cecil.Cil.Instruction instruction)
        {
            var interceptorVariable = new VariableDefinition(method.Module.Import(aspectType));
            _aspectInstanceBuilder.CreateAspectInstance(aspectType, lifeCycle, method, interceptorVariable, instructionsToInsert_P.aspectInitialisation, customAttribute);
            return interceptorVariable;
        }

        public void Check(MethodDefinition method, ErrorHandler errorHandler)
        {
            if (method.IsStatic && lifeCycle == LifeCycle.PerInstance)
                errorHandler.OnError(ErrorCode.ImpossibleToHavePerInstanceLifeCycleInStaticMethod, FileLocation.None, aspectType.FullName, method.Name, method.DeclaringType.FullName.Replace("/", "+"));
        }
    }
}