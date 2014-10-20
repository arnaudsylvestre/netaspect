using Mono.Cecil;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Model
{
   public class ParameterWeavingInfo : MethodWeavingInfo
   {
      public ParameterDefinition Parameter { get; set; }
   }
}
