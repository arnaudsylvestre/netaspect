using System.Collections.Generic;
using NetAspect.Weaver.Core.Weaver;
using NetAspect.Weaver.Core.Weaver.Generators;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Model.Weaving
{
    public class AroundMethodWeavingModel
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