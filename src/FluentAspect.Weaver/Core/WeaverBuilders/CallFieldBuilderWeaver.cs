using System.Collections.Generic;
using System.Linq;
using FluentAspect.Weaver.Core.Configuration;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Core.Model.Adapters;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Fields;
using FluentAspect.Weaver.Helpers;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.WeaverBuilders
{
    public class CallFieldBuilderWeaver : IWeaverBuilder
    {
        public IEnumerable<IWeaveable> BuildWeavers(WeavingConfiguration configuration)
        {
           var updates = new Dictionary<JoinPoint, List<CallWeavingConfiguration>>();
           var getters = new Dictionary<JoinPoint, List<CallWeavingConfiguration>>();

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
                            if (IsUpdateFieldInstruction(instruction))
                            {
                                //foreach (MethodMatch methodMatch in configuration.Methods)v
                                var methodMatch = field;
                                {
                                    if (methodMatch.Matcher(instruction.Operand as FieldReference))
                                    {
                                        if (methodMatch.Aspect != null)
                                        {
                                           if (methodMatch.Aspect.BeforeUpdateFieldValue.Method != null ||
                                                methodMatch.Aspect.AfterUpdateFieldValue.Method != null)
                                        {
                                           var methodPoint_L = new JoinPoint
                                              {
                                                 Method = method, Instruction = instruction,
                                              };
                                           if (!updates.ContainsKey(methodPoint_L))
                                           {
                                               updates.Add(methodPoint_L, new List<CallWeavingConfiguration>());
                                           }
                                           updates[methodPoint_L].Add(new CallWeavingConfiguration()
                                               {
                                                   Type = methodMatch.Aspect.Type,
                                                   After = methodMatch.Aspect.AfterUpdateFieldValue,
                                                   Before = methodMatch.Aspect.BeforeUpdateFieldValue,
                                               });
                                           
                                        }


                                        }
                                    }
                                }
                            }

                            if (IsGetFieldInstruction(instruction))
                            {
                                //foreach (MethodMatch methodMatch in configuration.Methods)v
                                var methodMatch = field;
                                {
                                    if (methodMatch.Matcher(instruction.Operand as FieldReference))
                                    {
                                        if (methodMatch.Aspect != null)
                                        {
                                            if (methodMatch.Aspect.BeforeUpdateFieldValue.Method != null ||
                                                 methodMatch.Aspect.AfterUpdateFieldValue.Method != null)
                                            {
                                                var methodPoint_L = new JoinPoint
                                                {
                                                    Method = method,
                                                    Instruction = instruction,
                                                };
                                                if (!getters.ContainsKey(methodPoint_L))
                                                {
                                                    getters.Add(methodPoint_L, new List<CallWeavingConfiguration>());
                                                }
                                                getters[methodPoint_L].Add(new CallWeavingConfiguration()
                                                {
                                                    Type = methodMatch.Aspect.Type,
                                                    After = methodMatch.Aspect.AfterUpdateFieldValue,
                                                    Before = methodMatch.Aspect.BeforeUpdateFieldValue,
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

            List<IWeaveable> weavables = new List<IWeaveable>();
            weavables.AddRange(updates.Select(point_L => new AroundInstructionWeaver(point_L.Key, new UpdateFieldWeaver(new FieldToWeave()
                {
                    JoinPoint = point_L.Key, Interceptors = point_L.Value
                }))).Cast<IWeaveable>());
            weavables.AddRange(getters.Select(point_L => new GetValueFieldWeaver(point_L.Key, point_L.Value)).Cast<IWeaveable>());
            return weavables;
        }

        private static bool IsUpdateFieldInstruction(Instruction instruction)
        {
            return instruction.OpCode == OpCodes.Stfld && instruction.Operand is FieldReference
                || instruction.OpCode == OpCodes.Stsfld && instruction.Operand is FieldReference;
        }
        private static bool IsGetFieldInstruction(Instruction instruction)
        {
            return instruction.OpCode == OpCodes.Ldflda && instruction.Operand is FieldReference ||
                 instruction.OpCode == OpCodes.Ldfld && instruction.Operand is FieldReference ||
                 instruction.OpCode == OpCodes.Ldsfld && instruction.Operand is FieldReference ||
                 instruction.OpCode == OpCodes.Ldsflda && instruction.Operand is FieldReference;
        }
    }
}