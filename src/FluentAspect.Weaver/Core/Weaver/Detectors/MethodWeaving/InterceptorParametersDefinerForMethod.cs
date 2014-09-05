using System;
using System.IO;
using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Weaver.Data.Variables;
using NetAspect.Weaver.Core.Weaver.Detectors.Engine;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving
{
   public static class InterceptorParametersDefinerForMethod
   {
      public static InstructionWeavingInfo AddCalled(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForInstruction> interceptorParameterConfigurations_P, IMemberDefinition member)
      {
         interceptorParameterConfigurations_P.AddPossibleParameter("called")
            .WhichCanNotBeReferenced()
            .WhereFieldCanNotBeStatic(member)
            .WhichMustBeOfTypeOf(member.DeclaringType)
            .AndInjectTheCalledInstance();
         return weavingInfo_P;
      }

      public static InstructionWeavingInfo AddCaller(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForInstruction> interceptorParameterConfigurations_P)
      {
         interceptorParameterConfigurations_P.AddPossibleParameter("caller")
            .WhichCanNotBeReferenced()
            .WhereCurrentMethodCanNotBeStatic(weavingInfo_P)
            .OrOfCurrentMethodDeclaringType(weavingInfo_P)
            .AndInjectTheCurrentInstance();
         return weavingInfo_P;
      }

      public static InstructionWeavingInfo AddCallerMethod(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForInstruction> interceptorParameterConfigurations_P)
      {
         interceptorParameterConfigurations_P.AddPossibleParameter("callermethod")
            .WhichCanNotBeReferenced()
            .WhichMustBeOfType<VariablesForInstruction, MethodBase>()
            .AndInjectTheCurrentMethod();
         return weavingInfo_P;
      }

      public static InstructionWeavingInfo AddCallerParameters(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForInstruction> interceptorParameterConfigurations_P)
      {
         interceptorParameterConfigurations_P.AddPossibleParameter("callerparameters")
            .WhichCanNotBeReferenced()
            .WhichMustBeOfType<VariablesForInstruction, object[]>()
            .AndInjectTheVariable(variables => variables.Parameters.Definition);
         return weavingInfo_P;
      }

      public static InstructionWeavingInfo AddCalledParameters(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForInstruction> interceptorParameterConfigurations_P)
      {
         interceptorParameterConfigurations_P.AddPossibleParameter("calledparameters")
            .WhichCanNotBeReferenced()
            .WhichMustBeOfType<VariablesForInstruction, object[]>()
            .AndInjectTheVariable(variables => variables.Parameters.Definition);
         return weavingInfo_P;
      }

      public static InstructionWeavingInfo AddCalledParameterNames(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForInstruction> interceptorParameterConfigurations_P)
      {
         foreach (ParameterDefinition parameterDefinition in weavingInfo_P.GetOperandAsMethod().Parameters)
         {
            interceptorParameterConfigurations_P.AddPossibleParameter("called" + parameterDefinition.Name.ToLower())
               .WhichCanNotBeReferenced()
               .WhichMustBeOfTypeOfParameter(parameterDefinition)
               .AndInjectTheCalledParameter(parameterDefinition);
         }
         return weavingInfo_P;
      }

      public static InstructionWeavingInfo AddCallerParameterNames(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForInstruction> interceptorParameterConfigurations_P)
      {
         foreach (ParameterDefinition parameter in weavingInfo_P.Method.Parameters)
         {
            interceptorParameterConfigurations_P.AddPossibleParameter("caller" + parameter.Name.ToLower())
               .WhichCanNotBeOut()
               .WhichMustBeOfTypeOfParameter(parameter)
               .AndInjectTheParameter(parameter);
         }
         return weavingInfo_P;
      }

      public static InstructionWeavingInfo AddColumnNumber(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForInstruction> interceptorParameterConfigurations_P)
      {
         interceptorParameterConfigurations_P.AddPossibleParameter("columnnumber")
            .WhichCanNotBeReferenced()
            .WhichPdbPresent(weavingInfo_P)
            .WhichMustBeOfType<VariablesForInstruction, int>()
            .AndInjectThePdbInfo(s => s.StartColumn, weavingInfo_P);
         return weavingInfo_P;
      }

      public static InstructionWeavingInfo AddLineNumber(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForInstruction> interceptorParameterConfigurations_P)
      {
         interceptorParameterConfigurations_P.AddPossibleParameter("linenumber")
            .WhichCanNotBeReferenced()
            .WhichPdbPresent(weavingInfo_P)
            .WhichMustBeOfType<VariablesForInstruction, int>()
            .AndInjectThePdbInfo(s => s.StartLine, weavingInfo_P);
         return weavingInfo_P;
      }

      public static InstructionWeavingInfo AddFilePath(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForInstruction> interceptorParameterConfigurations_P)
      {
         interceptorParameterConfigurations_P.AddPossibleParameter("filepath")
            .WhichCanNotBeReferenced()
            .WhichPdbPresent(weavingInfo_P)
            .WhichMustBeOfType<VariablesForInstruction, string>()
            .AndInjectThePdbInfo(s => s.Document.Url, weavingInfo_P);
         return weavingInfo_P;
      }

      public static InstructionWeavingInfo AddFileName(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForInstruction> interceptorParameterConfigurations_P)
      {
         interceptorParameterConfigurations_P.AddPossibleParameter("filename")
            .WhichCanNotBeReferenced()
            .WhichPdbPresent(weavingInfo_P)
            .WhichMustBeOfType<VariablesForInstruction, string>()
            .AndInjectThePdbInfo(s => Path.GetFileName(s.Document.Url), weavingInfo_P);
         return weavingInfo_P;
      }

      public static InstructionWeavingInfo AddCalledFieldInfo(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForInstruction> interceptorParameterConfigurations_P)
      {
         interceptorParameterConfigurations_P.AddPossibleParameter("field")
            .WhichCanNotBeReferenced()
            .WhichMustBeOfType<VariablesForInstruction, FieldInfo>()
            .AndInjectTheCalledFieldInfo(weavingInfo_P);
         return weavingInfo_P;
      }

      public static InstructionWeavingInfo AddCalledPropertyInfo(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForInstruction> interceptorParameterConfigurations_P)
      {
         interceptorParameterConfigurations_P.AddPossibleParameter("property")
            .WhichCanNotBeReferenced()
            .WhichMustBeOfType<VariablesForInstruction, PropertyInfo>()
            .AndInjectTheCalledPropertyInfo(weavingInfo_P);
         return weavingInfo_P;
      }

      public static InstructionWeavingInfo AddException(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForMethod> interceptorParameterConfigurations_P)
      {
         interceptorParameterConfigurations_P.AddPossibleParameter("exception")
            .WhichCanNotBeReferenced()
            .WhichMustBeOfType<VariablesForMethod, Exception>()
            .AndInjectTheVariable(variables => variables.Exception.Definition);
         return weavingInfo_P;
      }

      public static InstructionWeavingInfo AddResult(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForMethod> interceptorParameterConfigurations_P)
      {
         interceptorParameterConfigurations_P.AddPossibleParameter("exception")
            .WhereParameterTypeIsSameAsMethodResult(weavingInfo_P)
            .AndInjectTheVariable(variables => variables.Result.Definition);
         return weavingInfo_P;
      }
   }
}
