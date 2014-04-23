using NetAspect.Weaver.Core.Weaver.Detectors;
using NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Field;
using NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Method;
using NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Property;
using NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving;
using NetAspect.Weaver.Core.Weaver.Engine;

namespace NetAspect.Weaver
{
   public static class WeaverFactory
   {
      public static WeaverCore Create()
      {
         return new WeaverCore(
            new WeavingModelComputer(
               new MultiWeavingDetector(
                  new MethodAttributeWeavingDetector(),
                  new PropertyGetAttributeWeavingDetector(),
                  new CallMethodInstructionWeavingDetector(),
                  new CallGetFieldInstructionWeavingDetector(),
                  new CallUpdateFieldInstructionWeavingDetector(),
                  new CallGetPropertyInstructionWeavingDetector(),
                  new ConstructorAttributeWeavingDetector())));
      }
   }
}
