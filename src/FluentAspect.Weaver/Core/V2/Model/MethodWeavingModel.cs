using System.Collections.Generic;

namespace FluentAspect.Weaver.Core.V2
{
    public class MethodWeavingModel
   {
      public List<IIlInjector> Befores = new List<IIlInjector>();
      public List<IIlInjector> Afters = new List<IIlInjector>();
      public List<IIlInjector> OnExceptions = new List<IIlInjector>();
      public List<IIlInjector> OnFinallys = new List<IIlInjector>();
   }
}