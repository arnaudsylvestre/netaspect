using System.Collections.Generic;
using NetAspect.Weaver.Core.Weaver;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Model.Weaving
{
    public class AroundMethodWeavingModel
    {
        public List<IIlInjector> Afters = new List<IIlInjector>();
        public List<IIlInjector> Befores = new List<IIlInjector>();
        public List<IIlInjector> OnExceptions = new List<IIlInjector>();
        public List<IIlInjector> OnFinallys = new List<IIlInjector>();
    }
}