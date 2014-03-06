using System.Collections.Generic;

namespace FluentAspect.Weaver.Core.V2
{
    public class MethodWeavingModel
   {
        public List<IIlInjector<IlInjectorAvailableVariables>> Befores = new List<IIlInjector<IlInjectorAvailableVariables>>();
        public List<IIlInjector<IlInjectorAvailableVariables>> Afters = new List<IIlInjector<IlInjectorAvailableVariables>>();
        public List<IIlInjector<IlInjectorAvailableVariables>> OnExceptions = new List<IIlInjector<IlInjectorAvailableVariables>>();
        public List<IIlInjector<IlInjectorAvailableVariables>> OnFinallys = new List<IIlInjector<IlInjectorAvailableVariables>>();
   }
}