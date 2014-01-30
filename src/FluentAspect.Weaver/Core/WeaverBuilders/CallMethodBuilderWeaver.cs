using System.Collections.Generic;
using System.Linq;
using FluentAspect.Weaver.Core.Configuration;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Core.Model.Adapters;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Factory;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Methods;
using FluentAspect.Weaver.Helpers;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.WeaverBuilders
{
    public class CallMethodBuilderWeaver : IWeaverBuilder
    {
        public IEnumerable<IWeaveable> BuildWeavers(WeavingConfiguration configuration)
        {
           var points = new Dictionary<JoinPoint, List<CallWeavingConfiguration>>();

           //var methodMatches = new List<MethodMatch>(configuration.Constructors);
           var methodMatches = new List<AspectMatch<IMethod>>(configuration.Methods);
           methodMatches.AddRange(configuration.Constructors);
            foreach (var m in methodMatches)
            {
                foreach (var assemblyDefinition in m.AssembliesToScan)
                {
                   List<MethodDefinition> methods = assemblyDefinition.GetAllMethods();
                    foreach (MethodDefinition method in methods)
                    {
                        if (!method.HasBody)
                            continue;
                        foreach (Instruction instruction in method.Body.Instructions)
                        {
                            if (instruction.OpCode == OpCodes.Call && instruction.Operand is MethodReference ||
                                instruction.OpCode == OpCodes.Newobj && instruction.Operand is MethodReference)
                            {
                                //foreach (MethodMatch methodMatch in configuration.Methods)v
                                var methodMatch = m;
                                {
                                    if (methodMatch.Matcher(new MethodDefinitionAdapter(instruction.Operand as MethodReference)))
                                    {

                                        if (methodMatch.Aspect != null)
                                        {
                                            if (methodMatch.Aspect.BeforeInterceptor.Method != null ||
                                                methodMatch.Aspect.AfterInterceptor.Method != null)
                                        {
                                           var methodPoint_L = new JoinPoint
                                              {
                                                 Method = method, InstructionStart = instruction,
                                                 InstructionEnd                                                 = instruction,
                                              };
                                           if (!points.ContainsKey(methodPoint_L))
                                           {
                                              points.Add(methodPoint_L, new List<CallWeavingConfiguration>());
                                           }
                                           points[methodPoint_L].Add(new CallWeavingConfiguration()
                                           {
                                               Type = methodMatch.Aspect.Type,
                                               After = methodMatch.Aspect.AfterInterceptor,
                                               Before = methodMatch.Aspect.BeforeInterceptor,
                                           });
                                           
                                        }


                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }


           return points.Select(point_L => new AroundInstructionWeaver(point_L.Key, new CallMethodWeaver(new MethodCallToWeave()
               {
                   JoinPoint = point_L.Key, Interceptors = point_L.Value
               }))).Cast<IWeaveable>().ToList();
        }
    }
}