﻿using System;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Helpers.Mono.Cecil.IL;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables.Method
{
   public class VariableCurrentMethodBuilder : Variable.IVariableBuilder
   {
       public void Check(MethodDefinition method, ErrorHandler errorHandler)
       {
           
       }

       public VariableDefinition Build(InstructionsToInsert instructionsToInsert_P, MethodDefinition method, Mono.Cecil.Cil.Instruction instruction)
      {
         var variable = new VariableDefinition(method.Module.Import(typeof(MethodBase)));
         var methodInfo_L = typeof(MethodBase).GetMethod("GetCurrentMethod",new Type[] { });
         instructionsToInsert_P.BeforeInstructions.AppendCallStaticMethodAnsSaveResultInto(methodInfo_L, variable, method.Module);
         return variable;
      }
   }
}