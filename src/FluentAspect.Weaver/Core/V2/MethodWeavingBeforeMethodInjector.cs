using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Methods;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.V2
{
    public class MethodWeavingBeforeMethodInjector : IIlInjector
    {
       private readonly MethodDefinition _method;
       private MethodInfo interceptorMethod;
       private ParametersEngine forBeforeMethodWeaving_L;

       public MethodWeavingBeforeMethodInjector(MethodDefinition method_P, MethodInfo interceptorMethod_P)
       {
          _method = method_P;
          interceptorMethod = interceptorMethod_P;
       }

       public void Check(ErrorHandler errorHandler, IlInjectorAvailableVariables availableInformations)
       {
          forBeforeMethodWeaving_L = ParametersEngineFactory.CreateForBeforeMethodWeaving(_method, () => availableInformations.CurrentMethodInfo, null, errorHandler);
          forBeforeMethodWeaving_L.Check(interceptorMethod.GetParameters(), errorHandler);
       }

       public void Inject(List<Instruction> instructions, IlInjectorAvailableVariables availableInformations)
       {
          forBeforeMethodWeaving_L.Fill(interceptorMethod.GetParameters(), instructions);
        }
    }
}