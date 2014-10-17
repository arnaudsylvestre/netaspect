using Mono.Cecil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Data.Variables;

namespace NetAspect.Weaver.Core.Weaver.Engine.Instructions
{
    public class AspectInstanceForInstruction
   {
       public CustomAttribute Instance { get; set; }
       public NetAspectDefinition Aspect { get; set; }
       public IIlInjector<VariablesForInstruction> After { get; set; }
       public IIlInjector<VariablesForInstruction> Before { get; set; }

   }
}
