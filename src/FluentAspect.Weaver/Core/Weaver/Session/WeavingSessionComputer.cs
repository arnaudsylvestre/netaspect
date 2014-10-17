using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Assemblies;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Helpers;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Helpers;

namespace NetAspect.Weaver.Core.Weaver.Engine
{
    public class WeavingSessionComputer
    {
        private readonly IAspectChecker _aspectChecker;
        private readonly IAspectFinder _aspectFinder;
        private readonly List<IInstructionAspectInstanceDetector> instructionWeavingDetector;
        private readonly List<IMethodAspectInstanceDetector> methodWeavingDetector;

        public WeavingSessionComputer(IAspectFinder aspectFinder,
                                      IAspectChecker aspectChecker,
                                      List<IInstructionAspectInstanceDetector> instructionWeavingDetector,
                                      List<IMethodAspectInstanceDetector> methodWeavingDetector)
        {
            _aspectFinder = aspectFinder;
            _aspectChecker = aspectChecker;
            this.instructionWeavingDetector = instructionWeavingDetector;
            this.methodWeavingDetector = methodWeavingDetector;
        }

        public WeavingSession ComputeWeavingSession(Type[] typesP_L,
                                                    Type[] filter,
                                                    AssemblyPool assemblyPool,
                                                    ErrorHandler errorHandler)
        {
            var aspects = _aspectFinder.Find(typesP_L);
            aspects.ForEach(aspect => _aspectChecker.Check(aspect, errorHandler));
            var assembliesToWeave = aspects.GetAssembliesToWeave(typesP_L[0].Assembly);
            return ComputeWeavingSession(assembliesToWeave.ToList(), filter, assemblyPool, aspects);
        }

        private WeavingSession ComputeWeavingSession(List<Assembly> assembliesToWeave,
                                                     Type[] filter,
                                                     AssemblyPool assemblyPool,
                                                     List<NetAspectDefinition> aspects)
        {
            var session = new WeavingSession();
            assemblyPool.Add(assembliesToWeave);
            foreach (var assembly in assembliesToWeave)
            {
                FillSessionForAssembly(filter, assemblyPool, aspects, assembly, session);
            }
            return session;
        }

        private void FillSessionForAssembly(Type[] filter, AssemblyPool assemblyPool,
                                            List<NetAspectDefinition> aspects, Assembly assembly,
                                            WeavingSession session)
        {
            foreach (var method in assemblyPool.GetAssemblyDefinition(assembly).GetAllMethodsWithBody(filter))
            {
                FillSessionForMethod(aspects, session, method);
            }
        }

        private void FillSessionForMethod(IEnumerable<NetAspectDefinition> aspects, WeavingSession session, MethodDefinition method)
        {
            foreach (var aspect in aspects)
            {
                methodWeavingDetector.ForEach(model => session.Add(method, model.GetAspectInstances(method, aspect)));
                foreach (var instruction in method.Body.Instructions)
                {
                    instructionWeavingDetector.ForEach(
                        model => session.Add(
                            method,
                            instruction,
                            model.GetAspectInstances(method, instruction, aspect)));
                }
            }
        }

        public interface IAspectChecker
        {
            void Check(NetAspectDefinition aspect, ErrorHandler errorHandler);
        }

        public interface IAspectFinder
        {
            List<NetAspectDefinition> Find(IEnumerable<Type> types);
        }
    }
}