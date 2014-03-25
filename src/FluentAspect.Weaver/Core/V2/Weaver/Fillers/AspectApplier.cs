using System.Linq;
using FluentAspect.Weaver.Core.Model;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.V2.Weaver.Fillers
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