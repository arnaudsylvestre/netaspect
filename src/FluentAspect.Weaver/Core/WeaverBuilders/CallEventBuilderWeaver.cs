using System.Collections.Generic;
using System.Linq;
using FluentAspect.Weaver.Core.Configuration;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Events;
using FluentAspect.Weaver.Helpers;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.WeaverBuilders
{
    public class CallEventBuilderWeaver : IWeaverBuilder
    {
        public IEnumerable<IWeaveable> BuildWeavers(WeavingConfiguration configuration)
        {
            var updates = new Dictionary<JoinPoint, List<CallWeavingConfiguration>>();

            //var methodMatches = new List<MethodMatch>(configuration.Constructors);
            List<AspectMatch<FieldReference>> matches = configuration.Events;
            foreach (var evt in matches)
            {
                foreach (AssemblyDefinition assemblyDefinition in evt.AssembliesToScan)
                {
                    List<MethodDefinition> methods = assemblyDefinition.GetAllMethods();
                    foreach (MethodDefinition method in methods)
                    {
                        if (!method.HasBody)
                            continue;
                        foreach (Instruction instruction in method.Body.Instructions)
                        {
                            if (IsCallInstruction(instruction))
                            {
                                //foreach (MethodMatch methodMatch in configuration.Methods)v
                                AspectMatch<FieldReference> methodMatch = evt;
                                {
                                    if (methodMatch.Matcher(instruction.Operand as FieldReference))
                                    {
                                        if (methodMatch.Aspect != null)
                                        {
                                            if (methodMatch.Aspect.BeforeCallEvent.Method != null ||
                                                methodMatch.Aspect.AfterCallEvent.Method != null)
                                            {
                                                var methodPoint_L = new JoinPoint
                                                    {
                                                        Method = method,
                                                        InstructionStart = instruction,
                                                        InstructionEnd = instruction.Next,
                                                    };
                                                if (!updates.ContainsKey(methodPoint_L))
                                                {
                                                    updates.Add(methodPoint_L, new List<CallWeavingConfiguration>());
                                                }
                                                updates[methodPoint_L].Add(new CallWeavingConfiguration
                                                    {
                                                        Type = methodMatch.Aspect.Type,
                                                        After = methodMatch.Aspect.AfterCallEvent,
                                                        Before = methodMatch.Aspect.BeforeCallEvent,
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

            var weavables = new List<IWeaveable>();
            weavables.AddRange(
                updates.Select(
                    point_L => new AroundInstructionWeaver(point_L.Key, new UpdateCallEventWeaver(new EventCallToWeave
                        {
                            JoinPoint = point_L.Key,
                            Interceptors = point_L.Value
                        }))).Cast<IWeaveable>());
            return weavables;
        }

        private static bool IsCallInstruction(Instruction instruction)
        {
            return
                (instruction.OpCode == OpCodes.Ldfld && instruction.Operand is FieldReference ||
                 instruction.OpCode == OpCodes.Ldsfld && instruction.Operand is FieldReference) &&
                IsCallInvoke(instruction.Next);
        }

        private static bool IsCallInvoke(Instruction instruction)
        {
            return
                (instruction.OpCode == OpCodes.Call && instruction.Operand is MethodReference ||
                 instruction.OpCode == OpCodes.Callvirt && instruction.Operand is MethodReference) &&
                (instruction.Operand as MethodReference).Name == "Invoke";
        }
    }
}