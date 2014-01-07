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
            var weavers = new List<IWeaveable>();

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
                                foreach (MethodMatch methodMatch in configuration.Methods)
                                {
                                    if (methodMatch.Matcher(new MethodDefinitionAdapter(instruction.Operand as MethodReference)))
                                    {
                                        var actualInterceptors = new List<CallWeavingConfiguration>();

                                        foreach (var interceptorType in methodMatch.CallWeavingInterceptors)
                                        {
                                            if (interceptorType.BeforeInterceptor.Method != null ||
                                                interceptorType.AfterInterceptor.Method != null)
                                                actualInterceptors.Add(interceptorType);
                                        }
                                        if (actualInterceptors.Count != 0)
                                            weavers.Add(new CallMethodWeaver(method, instruction, methodMatch.CallWeavingInterceptors));
                                    }
                                }
                            }
                        }
                    }
                }

            }


            

            return weavers;
        }
    }
}