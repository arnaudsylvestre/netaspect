using System.Linq;
using Mono.Cecil;
using NetAspect.Weaver.Core.Model.Aspect;

namespace NetAspect.Weaver.Core.Weaver.Detectors.Helpers
{
    public class AspectApplier
    {
        public static bool CanApply(FieldDefinition field, NetAspectDefinition netAspect)
        {
            TypeReference aspectType = field.Module.Import(netAspect.Type);
            bool compliant = field.CustomAttributes.Any(
                customAttribute_L =>
                customAttribute_L.AttributeType.FullName == aspectType.FullName);
            if (compliant)
                return true;
            if (netAspect.FieldSelector.IsCompliant(field))
                return true;
            return false;
        }
    }
}