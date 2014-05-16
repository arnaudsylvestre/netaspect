using Mono.Cecil;

namespace NetAspect.Weaver.Core.Weaver.Detectors.Model
{
   public class ParameterWeavingInfo : MethodWeavingInfo
   {
      public ParameterDefinition Parameter { get; set; }
   }
}