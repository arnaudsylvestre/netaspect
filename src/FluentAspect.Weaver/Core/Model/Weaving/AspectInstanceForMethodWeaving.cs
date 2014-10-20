using System.Collections.Generic;
using Mono.Cecil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;
using NetAspect.Weaver.Core.Weaver.ToSort.ILInjector;

namespace NetAspect.Weaver.Core.Model.Weaving
{
   public class AspectInstanceForMethodWeaving
   {
      public List<IIlInjector<VariablesForMethod>> BeforeConstructorBaseCalls = new List<IIlInjector<VariablesForMethod>>();
      public List<IIlInjector<VariablesForMethod>> Befores = new List<IIlInjector<VariablesForMethod>>();
      public List<IIlInjector<VariablesForMethod>> Afters = new List<IIlInjector<VariablesForMethod>>();
      public List<IIlInjector<VariablesForMethod>> OnExceptions = new List<IIlInjector<VariablesForMethod>>();
      public List<IIlInjector<VariablesForMethod>> OnFinallys = new List<IIlInjector<VariablesForMethod>>();

       public CustomAttribute Instance { get; set; }
       public NetAspectDefinition Aspect { get; set; }
   }
}
