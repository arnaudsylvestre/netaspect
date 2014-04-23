using System.Collections.Generic;
using NetAspect.Weaver.Core.Weaver;
using NetAspect.Weaver.Core.Weaver.Generators;

namespace NetAspect.Weaver.Core.Model.Weaving
{
    public class MethodWeavingModel
    {
        public List<IIlInjector> Afters =
            new List<IIlInjector>();

        public List<IIlInjector> Befores =
            new List<IIlInjector>();

        public List<IIlInjector> OnExceptions =
            new List<IIlInjector>();

        public List<IIlInjector> OnFinallys =
            new List<IIlInjector>();
    }
}