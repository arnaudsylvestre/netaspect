using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Detectors.Engine;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method
{
    public interface IWevingPreconditionInjector
    {
        void Inject(List<Instruction> precondition, IlInjectorAvailableVariables availableInformations);
    }

    public class OverrideWevingPreconditionInjector : IWevingPreconditionInjector
    {
        public void Inject(List<Instruction> precondition, IlInjectorAvailableVariables availableInformations)
        {
        //    IL_0000: nop
        //IL_0001: ldstr "NetAspect.Weaver.Tests.unit.InstructionWeaving.Methods.Parameters.After.Called.OverrideTest+A"
        //IL_0006: ldarg.0
        //IL_0007: callvirt instance class [mscorlib]System.Type [mscorlib]System.Object::GetType()
        //IL_000c: ldstr "Method"
        //IL_0011: callvirt instance class [mscorlib]System.Reflection.MethodInfo [mscorlib]System.Type::GetMethod(string)
        //IL_0016: callvirt instance class [mscorlib]System.Type [mscorlib]System.Reflection.MemberInfo::get_DeclaringType()
        //IL_001b: callvirt instance string [mscorlib]System.Type::get_FullName()
        //IL_0020: call bool [mscorlib]System.String::op_Equality(string, string)
        //IL_0025: stloc.0
        //IL_0026: br.s IL_0028

        //IL_0028: ldloc.0
        //IL_0029: ret
            throw new System.NotImplementedException();
        }
    }

    public class NoWevingPreconditionInjector : IWevingPreconditionInjector
    {
        public void Inject(List<Instruction> precondition, IlInjectorAvailableVariables availableInformations)
        {
            
        }
    }

   public class Injector : IIlInjector
   {
      private readonly MethodDefinition _method;
       private readonly MethodInfo interceptorMethod;
      private readonly InterceptorParameterConfigurations interceptorParameterConfigurations;
       private IWevingPreconditionInjector weavingPreconditionInjector;


       public Injector(MethodDefinition method_P, MethodInfo interceptorMethod_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P, IWevingPreconditionInjector weavingPreconditionInjector)
      {
         _method = method_P;
         interceptorMethod = interceptorMethod_P;
           interceptorParameterConfigurations = interceptorParameterConfigurations_P;
           this.weavingPreconditionInjector = weavingPreconditionInjector;
      }

      public void Check(ErrorHandler errorHandler)
      {
         interceptorParameterConfigurations.Check(interceptorMethod.GetParameters(), errorHandler);
      }

      public void Inject(List<Instruction> instructions, IlInjectorAvailableVariables availableInformations)
      {
          Instruction end = Instruction.Create(OpCodes.Nop);
          var precondition = new List<Instruction>();
          weavingPreconditionInjector.Inject(precondition, availableInformations);
          if (precondition.Any())
          {
              instructions.AddRange(precondition);
              instructions.Add(Instruction.Create(OpCodes.Brfalse, end));
          }
          instructions.Add(Instruction.Create(OpCodes.Ldloc, availableInformations.InterceptorVariable));
         ParametersIlGenerator.Generate(interceptorMethod.GetParameters(), instructions, availableInformations, interceptorParameterConfigurations);
         instructions.Add(Instruction.Create(OpCodes.Call, _method.Module.Import(interceptorMethod)));
          instructions.Add(end);
      }
   }
}
