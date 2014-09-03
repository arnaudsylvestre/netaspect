using System;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver.Core.Weaver.Data.Variables
{
   public class VariableCurrentMethodBuilder : IVariableBuilder
   {
      public VariableDefinition Build(InstructionsToInsert instructionsToInsert_P, MethodDefinition method)
      {
         var variable = new VariableDefinition(method.Module.Import(typeof(MethodBase)));
         var methodInfo_L = typeof(MethodBase).GetMethod("GetCurrentMethod",new Type[] { });
         instructionsToInsert_P.BeforeInstructions.AppendCallStaticMethodAnsSaveResultInto(methodInfo_L, variable, method.Module);
         return variable;
      }
   }
}