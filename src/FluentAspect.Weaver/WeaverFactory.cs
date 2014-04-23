﻿using NetAspect.Weaver.Apis.AssemblyChecker.Peverify;
using NetAspect.Weaver.Core.Weaver;
using NetAspect.Weaver.Core.Weaver.Detectors;
using NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Field;
using NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Method;
using NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Property;
using NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving;
using NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving.Constructor;
using NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving.Property;
using NetAspect.Weaver.Core.Weaver.Engine;
using NetAspect.Weaver.Core.Weaver.Engine.AspectFinders;
using NetAspect.Weaver.Core.Weaver.Engine.AssemblyPoolFactories;

namespace NetAspect.Weaver
{
   public static class WeaverFactory
   {
      public static WeaverEngine Create()
      {
         return new WeaverEngine(
            new WeavingModelComputer2(
               new WeavingModelComputer(
                  new MultiWeavingDetector(
                     new MethodAttributeWeavingDetector(),
                     new PropertyGetAttributeWeavingDetector(),
                     new CallMethodInstructionWeavingDetector(),
                     new CallGetFieldInstructionWeavingDetector(),
                     new CallUpdateFieldInstructionWeavingDetector(),
                     new CallGetPropertyInstructionWeavingDetector(),
                     new ConstructorAttributeWeavingDetector())),
                     new DefaultAspectFinder()),
            new DefaultAssemblyPoolFactory(new PeVerifyAssemblyChecker()));
      }
   }
}
