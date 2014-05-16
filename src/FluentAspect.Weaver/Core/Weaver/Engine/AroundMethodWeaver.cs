﻿using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.Engine
{
    public class AroundMethodWeaver
    {
        public AroundMethodWeavingModel Model = new AroundMethodWeavingModel();
        private MethodDefinition method;
        private NetAspectDefinition aspect;
        private AspectBuilder aspectBuilder;

        public AroundMethodWeaver(MethodDefinition method, NetAspectDefinition aspect, AspectBuilder aspectBuilder)
        {
            this.method = method;
            this.aspect = aspect;
            this.aspectBuilder = aspectBuilder;
        }

        public void Check(ErrorHandler errorHandler)
        {
            Model.Befores.Check(errorHandler);
            Model.Afters.Check(errorHandler);
            Model.OnExceptions.Check(errorHandler);
            Model.OnFinallys.Check(errorHandler);
        }

        public void Inject(List<Instruction> befores, List<Instruction> afters, List<Instruction> onExceptions, List<Instruction> onFinallys, IlInjectorAvailableVariables availableVariables)
        {
            Model.Befores.Inject(befores, availableVariables);
            Model.Afters.Inject(afters, availableVariables);
            Model.OnExceptions.Inject(onExceptions, availableVariables);
            Model.OnFinallys.Inject(onFinallys, availableVariables);
        }

        public VariableDefinition CreateAspect(List<Instruction> befores)
        {
            VariableDefinition interceptorVariable = new VariableDefinition(method.Module.Import(aspect.Type));
            aspectBuilder.CreateInterceptor(aspect, method, interceptorVariable, befores);
            return interceptorVariable;
        }
    }
}