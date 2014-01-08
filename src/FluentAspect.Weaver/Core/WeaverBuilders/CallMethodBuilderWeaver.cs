using System.Collections.Generic;
using System.Linq;
using FluentAspect.Weaver.Core.Configuration;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Core.Weavers.Calls;
using FluentAspect.Weaver.Helpers;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.WeaverBuilders
{
    public class CallMethodBuilderWeaver : IWeaverBuilder
    {
        public IEnumerable<IWeaveable> BuildWeavers(WeavingConfiguration configuration)
        {
           var points = new Dictionary<MethodPoint, List<CallWeavingConfiguration>>();

            foreach (MethodMatch m in configuration.Methods)
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
                            if (instruction.OpCode == OpCodes.Call && instruction.Operand is MethodReference)
                            {
                                //foreach (MethodMatch methodMatch in configuration.Methods)v
                                var methodMatch = m;
                                {
                                    if (methodMatch.Matcher(new MethodDefinitionAdapter(instruction.Operand as MethodReference)))
                                    {

                                        if (methodMatch.CallWeavingInterceptors != null)
                                        {
                                           if (methodMatch.CallWeavingInterceptors.BeforeInterceptor.Method != null ||
                                                methodMatch.CallWeavingInterceptors.AfterInterceptor.Method != null)
                                        {
                                           var methodPoint_L = new MethodPoint
                                              {
                                                 Method = method, Instruction = instruction,
                                              };
                                           if (!points.ContainsKey(methodPoint_L))
                                           {
                                              points.Add(methodPoint_L, new List<CallWeavingConfiguration>());
                                           }
                                           points[methodPoint_L].Add(methodMatch.CallWeavingInterceptors);
                                           
                                        }


                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }


           return points.Select(point_L => new CallMethodWeaver(point_L.Key, point_L.Value)).Cast<IWeaveable>().ToList();
        }
    }
}