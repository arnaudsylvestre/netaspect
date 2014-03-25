using System.Collections.Generic;
using FluentAspect.Weaver.Core.V2.Weaver;
using FluentAspect.Weaver.Core.V2.Weaver.Method;

namespace FluentAspect.Weaver.Core.V2.Model
{
    public class MethodWeavingModel
    {
        public List<IIlInjector<IlInjectorAvailableVariables>> Afters =
            new List<IIlInjector<IlInjectorAvailableVariables>>();

        public List<IIlInjector<IlInjectorAvailableVariables>> Befores =
            new List<IIlInjector<IlInjectorAvailableVariables>>();

        public List<IIlInjector<IlInjectorAvailableVariables>> OnExceptions =
            new List<IIlInjector<IlInjectorAvailableVariables>>();

        public List<IIlInjector<IlInjectorAvailableVariables>> OnFinallys =
            new List<IIlInjector<IlInjectorAvailableVariables>>();
    }
}