using System.Linq;
using Mono.Cecil;
using Mono.Collections.Generic;
using NetAspect.Weaver.Core.Model.Aspect;

namespace NetAspect.Weaver.Core.Weaver.Detectors.Engine
{
    public static class AspectApplier
    {

       public static bool CanApply<T>(T member, NetAspectDefinition netAspect, SelectorProvider<T> selectorProvider)
           where T : MemberReference, ICustomAttributeProvider
       {
          return CanApply(member, netAspect, selectorProvider, member.Module, member.DeclaringType.Resolve());
       }
       private static bool CanApply<T>(T member, NetAspectDefinition netAspect, SelectorProvider<T> selectorProvider, ModuleDefinition module, TypeDefinition declaringType)
           where T : ICustomAttributeProvider
       {
          TypeReference aspectType = module.Import(netAspect.Type);
          if (member.HasAspectAttribute(aspectType))
             return true;
          if (declaringType.HasAspectAttribute(aspectType))
             return true;
          return selectorProvider(netAspect).IsCompliant(member);
       }

       private static bool HasAspectAttribute(this ICustomAttributeProvider member, TypeReference aspectType)
       {
          if (member == null)
             return false;
          return member.CustomAttributes.Any(
             customAttribute_L =>
                customAttribute_L.AttributeType.FullName == aspectType.FullName);
       }

       public static bool CanApply<T>(T member, NetAspectDefinition netAspect, SelectorProvider<T> selectorProvider, ModuleDefinition module)
           where T : ICustomAttributeProvider
       {
          return CanApply(member, netAspect, selectorProvider, module, null);
       }

       
    }
}