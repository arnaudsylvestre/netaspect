using System;
using System.Collections.Generic;
using Mono.Cecil;
using NetAspect.Weaver.Apis.AssemblyChecker.Peverify;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Weaver;
using NetAspect.Weaver.Core.Weaver.Session;
using NetAspect.Weaver.Core.Weaver.Session.AspectCheckers;
using NetAspect.Weaver.Core.Weaver.Session.AspectFinders;
using NetAspect.Weaver.Core.Weaver.ToSort.Engine;
using NetAspect.Weaver.Core.Weaver.ToSort.Engine.AssemblyPoolFactories;
using NetAspect.Weaver.Core.Weaver.ToSort.Engine.LifeCycle;

namespace NetAspect.Weaver.Factory
{
   public static class WeaverFactory
   {
       public static WeaverEngine Create(Func<TypeDefinition, bool> typesToSave = null)
      {
           return new WeaverEngine(
            new WeavingSessionComputer(
               new DefaultAspectFinder(),
               new DefaultAspectChecker(),
               new List<IInstructionAspectInstanceDetector>
               {
                  InstructionWeavingDetectorFactory.BuildCallGetFieldDetector(),
                  InstructionWeavingDetectorFactory.BuildCallUpdateFieldDetector(),
                  InstructionWeavingDetectorFactory.BuildCallMethodDetector(),
                  InstructionWeavingDetectorFactory.BuildCallConstructorDetector(),
                  InstructionWeavingDetectorFactory.BuildCallGetPropertyDetector(),
                  InstructionWeavingDetectorFactory.BuildCallUpdatePropertyDetector(),
               },
               new List<IMethodAspectInstanceDetector>
               {
                  MethodWeavingDetectorFactory.BuildMethodDetector(),
                  MethodWeavingDetectorFactory.BuildPropertyGetterDetector(),
                  MethodWeavingDetectorFactory.BuildPropertyUpdaterDetector(),
                  MethodWeavingDetectorFactory.BuildConstructorDetector(),

                  MethodWeavingDetectorFactory.BuildMethodParameterDetector(),
                  MethodWeavingDetectorFactory.BuildConstructorParameterDetector(),
               }),
            new DefaultAssemblyPoolFactory(new PeVerifyAssemblyChecker(), typesToSave),
            new ErrorInfoComputer(ErrorsFactory.CreateAvailableErrors()
               ),
               new MethodWeaver(new AspectBuilder(LifeCyclesFactory.CreateLifeCycles())));
      }

       
   }
}
