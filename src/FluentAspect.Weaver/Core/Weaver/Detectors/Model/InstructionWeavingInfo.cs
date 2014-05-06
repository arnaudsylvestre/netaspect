using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving.Engine;
using NetAspect.Weaver.Core.Weaver.Generators;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving.Model
{
   public class InstructionWeavingInfo : MethodWeavingInfo
    {
        public Instruction Instruction { get; set; }
    }
}