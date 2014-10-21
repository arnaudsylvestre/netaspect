using Mono.Cecil;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Model
{
   public class ParameterWeavingInfo : CommonWeavingInfo
   {
      public ParameterDefinition Parameter { get; set; }
   }
}
