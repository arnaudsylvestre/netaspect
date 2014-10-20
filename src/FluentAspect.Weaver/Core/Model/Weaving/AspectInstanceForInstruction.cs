using Mono.Cecil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;
using NetAspect.Weaver.Core.Weaver.ToSort.ILInjector;

namespace NetAspect.Weaver.Core.Model.Weaving
{
    public class AspectInstanceForInstruction
   {
       public CustomAttribute Instance { get; set; }
       public NetAspectDefinition Aspect { get; set; }
       public IIlInjector<VariablesForInstruction> After { get; set; }
       public IIlInjector<VariablesForInstruction> Before { get; set; }

   }
}
