using System.Linq;
using Mono.Cecil;
using NetAspect.Weaver.Core.Model.Aspect;

namespace NetAspect.Weaver.Core.Weaver.Detectors.Engine
{
    public static class AspectApplier
    {

       public static bool CanApply<T>(T member, NetAspectDefinition netAspect, SelectorProvider<T> selectorProvider)
           where T : MemberReference, ICustomAttributeProvider
       {
          return CanApply(member, netAspect, selectorProvider, member.Module);
       }
       public static bool CanApply<T>(T member, NetAspectDefinition netAspect, SelectorProvider<T> selectorProvider, ModuleDefinition module)
           where T : ICustomAttributeProvider
       {
          TypeReference aspectType = module.Import(netAspect.Type);
          if (member.CustomAttributes.Any(
              customAttribute_L =>
              customAttribute_L.AttributeType.FullName == aspectType.FullName))
             return true;
          return selectorProvider(netAspect).IsCompliant(member);
       }

       
    }
}