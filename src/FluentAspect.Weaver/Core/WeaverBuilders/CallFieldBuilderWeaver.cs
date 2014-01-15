using System.Collections.Generic;
using System.Linq;
using FluentAspect.Weaver.Core.Configuration;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Core.Model.Adapters;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine;
using FluentAspect.Weaver.Helpers;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.WeaverBuilders
{
    public class CallFieldBuilderWeaver : IWeaverBuilder
    {
        public IEnumerable<IWeaveable> BuildWeavers(WeavingConfiguration configuration)
        {
           var points = new Dictionary<MethodPoint, List<CallWeavingConfiguration>>();

           //var methodMatches = new List<MethodMatch>(configuration.Constructors);
           var fieldMatches_L = configuration.Fields;
            foreach (var field in fieldMatches_L)
            {
                foreach (var assemblyDefinition in field.AssembliesToScan)
                {
                   List<MethodDefinition> methods = assemblyDefinition.GetAllMethods();
                    foreach (MethodDefinition method in methods)
                    {
                        if (!method.HasBody)
                            continue;
                        foreach (Instruction instruction in method.Body.Instructions)
                        {
                            if (IsAccessFieldInstruction(instruction))
                            {
                                //foreach (MethodMatch methodMatch in configuration.Methods)v
                                var methodMatch = field;
                                {
                                    if (methodMatch.Matcher(instruction.Operand as FieldReference))
                                    {
                                        if (methodMatch.CallWeavingInterceptors != null)
                                        {
                                           if (methodMatch.CallWeavingInterceptors.BeforeFieldAccess.Method != null ||
                                                methodMatch.CallWeavingInterceptors.AfterFieldAccess.Method != null)
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


           return points.Select(point_L => new CallFieldWeaver(point_L.Key, point_L.Value)).Cast<IWeaveable>().ToList();
        }

       private static bool IsAccessFieldInstruction(Instruction instruction)
       {
          return instruction.OpCode == OpCodes.Stfld && instruction.Operand is FieldReference ||
                  instruction.OpCode == OpCodes.Ldflda && instruction.Operand is FieldReference ||
                 instruction.OpCode == OpCodes.Ldfld && instruction.Operand is FieldReference;
       }
    }
}