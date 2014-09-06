using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Weaver.Data.Variables;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.Detectors.Engine
{
   public static class InterceptorParametersDefinerForInstruction
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

       public static MethodWeavingInfo AddCaller(this MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForInstruction> interceptorParameterConfigurations_P)
      {
         return AddCaller(weavingInfo_P, interceptorParameterConfigurations_P, "caller");
      }

       public static MethodWeavingInfo AddInstance(this MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForMethod> interceptorParameterConfigurations_P)
      {
         return AddCaller(weavingInfo_P, interceptorParameterConfigurations_P, "instance");
      }

       public static ParameterWeavingInfo AddParameterValue(this ParameterWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForMethod> interceptorParameterConfigurations_P)
      {
         interceptorParameterConfigurations_P.AddPossibleParameter("parametervalue")
            .WhichCanNotBeOut()
            .WhichMustBeOfTypeOfParameter(weavingInfo_P.Parameter)
            .AndInjectTheParameter(weavingInfo_P.Parameter);
         return weavingInfo_P;
      }

       public static ParameterWeavingInfo AddParameterName(this ParameterWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForMethod> interceptorParameterConfigurations_P)
      {
         interceptorParameterConfigurations_P.AddPossibleParameter("parametername")
            .WhichCanNotBeReferenced()
            .WhichMustBeOfType<VariablesForMethod, string>()
            .AndInjectTheValue(weavingInfo_P.Parameter.Name);
         return weavingInfo_P;
      }

      public static ParameterWeavingInfo AddParameterInfo(this ParameterWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForMethod> interceptorParameterConfigurations_P)
      {
         interceptorParameterConfigurations_P.AddPossibleParameter("parameter")
            .WhichCanNotBeReferenced()
            .WhichMustBeOfType<VariablesForMethod, ParameterInfo>()
            .AndInjectTheParameterInfo(weavingInfo_P.Parameter, weavingInfo_P.Method);
         return weavingInfo_P;
      }

      private static MethodWeavingInfo AddCaller<T>(MethodWeavingInfo weavingInfo_P,
         InterceptorParameterConfigurations<T> interceptorParameterConfigurations_P,
         string parameterName) where T : VariablesForMethod

      {
         interceptorParameterConfigurations_P.AddPossibleParameter(parameterName)
            .WhichCanNotBeReferenced()
            .WhereCurrentMethodCanNotBeStatic(weavingInfo_P)
            .OrOfCurrentMethodDeclaringType(weavingInfo_P)
            .AndInjectTheCurrentInstance();
         return weavingInfo_P;
      }

      public static MethodWeavingInfo AddCallerMethod(this MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForInstruction> interceptorParameterConfigurations_P)
      {
         return AddMethod(weavingInfo_P, interceptorParameterConfigurations_P, "callermethod");
      }

      private static MethodWeavingInfo AddMethod<T>(MethodWeavingInfo weavingInfo_P,
         InterceptorParameterConfigurations<T> interceptorParameterConfigurations_P,
         string parameterName)
          where T : VariablesForMethod
      {
         interceptorParameterConfigurations_P.AddPossibleParameter(parameterName)
            .WhichCanNotBeReferenced()
            .WhichMustBeOfType<T, MethodBase>()
            .AndInjectTheCurrentMethod();
         return weavingInfo_P;
      }

      public static MethodWeavingInfo AddProperty(this MethodWeavingInfo weavingInfo_P,
         InterceptorParameterConfigurations<VariablesForMethod> interceptorParameterConfigurations_P)
      {
         interceptorParameterConfigurations_P.AddPossibleParameter("property")
            .WhichCanNotBeReferenced()
            .WhichMustBeOfType<VariablesForMethod, PropertyInfo>()
            .AndInjectTheCurrentProperty();
         return weavingInfo_P;
      }

      public static MethodWeavingInfo AddMethod(this MethodWeavingInfo weavingInfo_P,
         InterceptorParameterConfigurations<VariablesForMethod> interceptorParameterConfigurations_P)
      {
         return AddMethod(weavingInfo_P, interceptorParameterConfigurations_P, "method");
      }

      public static MethodWeavingInfo AddConstructor(this MethodWeavingInfo weavingInfo_P,
         InterceptorParameterConfigurations<VariablesForMethod> interceptorParameterConfigurations_P)
      {
         return AddMethod(weavingInfo_P, interceptorParameterConfigurations_P, "constructor");
      }

      public static MethodWeavingInfo AddCallerParameters(this MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForInstruction> interceptorParameterConfigurations_P)
      {
         return AddCurrentParameters(weavingInfo_P, interceptorParameterConfigurations_P, "callerparameters");
      }

      public static MethodWeavingInfo AddParameters(this MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForMethod> interceptorParameterConfigurations_P)
      {
         return AddCurrentParameters(weavingInfo_P, interceptorParameterConfigurations_P, "parameters");
      }

      private static MethodWeavingInfo AddCurrentParameters<T>(MethodWeavingInfo weavingInfo_P,
         InterceptorParameterConfigurations<T>
            interceptorParameterConfigurations_P,
         string parameterName) where T : VariablesForMethod
      {
         interceptorParameterConfigurations_P.AddPossibleParameter(parameterName)
            .WhichCanNotBeReferenced()
            .WhichMustBeOfType<T, object[]>()
            .AndInjectTheVariable(variables => variables.Parameters.Definition);
         return weavingInfo_P;
      }

      public static InstructionWeavingInfo AddCalledParameters(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForInstruction> interceptorParameterConfigurations_P)
      {
         interceptorParameterConfigurations_P.AddPossibleParameter("calledparameters")
            .WhichCanNotBeReferenced()
            .WhichMustBeOfType<VariablesForInstruction, object[]>()
            .AndInjectTheVariable(variables => variables.CalledParametersObjects.Definition);
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

      public static MethodWeavingInfo AddCallerParameterNames(this MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForInstruction> interceptorParameterConfigurations_P)
      {
         return AddParameterNames(weavingInfo_P, interceptorParameterConfigurations_P, () => "caller{0}");
      }

      public static MethodWeavingInfo AddParameterNames<T>(this MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<T> interceptorParameterConfigurations_P) where T : VariablesForMethod
      {
         return AddParameterNames(weavingInfo_P, interceptorParameterConfigurations_P, () => "{0}");
      }

      private static MethodWeavingInfo AddParameterNames<T>(MethodWeavingInfo weavingInfo_P,
         InterceptorParameterConfigurations<T>
            interceptorParameterConfigurations_P,
         Func<string> parameterNameFormatProvider)
          where T : VariablesForMethod
      {
         foreach (ParameterDefinition parameter in weavingInfo_P.Method.Parameters)
         {
            interceptorParameterConfigurations_P.AddPossibleParameter(
               string.Format(
                  parameterNameFormatProvider(),
                  parameter.Name.ToLower()))
               .WhichCanNotBeOut()
               .WhichMustBeOfTypeOfParameter(parameter)
               .AndInjectTheParameter(parameter);
         }
         return weavingInfo_P;
      }


      public static MethodWeavingInfo AddValue(this MethodWeavingInfo weavingInfo_P,
         InterceptorParameterConfigurations<VariablesForMethod>
            interceptorParameterConfigurations_P)
      {
         ParameterDefinition parameter = weavingInfo_P.Method.Parameters[0];
         interceptorParameterConfigurations_P.AddPossibleParameter("value")
            .WhichCanNotBeOut()
            .WhichMustBeOfTypeOfParameter(parameter)
            .AndInjectTheParameter(parameter);
         return weavingInfo_P;
      }

      public static MethodWeavingInfo AddValueForInstruction(this InstructionWeavingInfo weavingInfo_P,
         InterceptorParameterConfigurations<VariablesForInstruction>
            interceptorParameterConfigurations_P)
      {
         ParameterDefinition parameter = weavingInfo_P.GetOperandAsMethod().Parameters.Last();
         interceptorParameterConfigurations_P.AddPossibleParameter("value")
            .WhichCanNotBeReferenced()
            .WhichMustBeOfTypeOfParameter(parameter)
            .AndInjectTheCalledValue(parameter);
         return weavingInfo_P;
      }

      public static MethodWeavingInfo AddFieldValue(this InstructionWeavingInfo weavingInfo_P,
         InterceptorParameterConfigurations<VariablesForInstruction>
            interceptorParameterConfigurations_P)
      {
         FieldDefinition field = weavingInfo_P.GetOperandAsField();
         TypeReference parameter = field.FieldType;
         interceptorParameterConfigurations_P.AddPossibleParameter("value")
            .WhichCanNotBeReferenced()
            .WhichMustBeOfTypeOf(parameter)
            .AndInjectTheFieldValue(field);
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

      public static MethodWeavingInfo AddLineNumberForMethod(this MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForMethod> interceptorParameterConfigurations_P)
      {
         interceptorParameterConfigurations_P.AddPossibleParameter("linenumber")
            .WhichCanNotBeReferenced()
            .WhichPdbPresentForMethod(weavingInfo_P)
            .WhichMustBeOfType<VariablesForMethod, int>()
            .AndInjectThePdbInfoForMethod(s => s.StartLine, weavingInfo_P);
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

      public static MethodWeavingInfo AddFilePathForMethod(this MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForMethod> interceptorParameterConfigurations_P)
      {
         interceptorParameterConfigurations_P.AddPossibleParameter("filepath")
            .WhichCanNotBeReferenced()
            .WhichPdbPresentForMethod(weavingInfo_P)
            .WhichMustBeOfType<VariablesForMethod, string>()
            .AndInjectThePdbInfoForMethod(s => s.Document.Url, weavingInfo_P);
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

      public static MethodWeavingInfo AddFileNameForMethod(this MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForMethod> interceptorParameterConfigurations_P)
      {
         interceptorParameterConfigurations_P.AddPossibleParameter("filename")
            .WhichCanNotBeReferenced()
            .WhichPdbPresentForMethod(weavingInfo_P)
            .WhichMustBeOfType<VariablesForMethod, string>()
            .AndInjectThePdbInfoForMethod(s => Path.GetFileName(s.Document.Url), weavingInfo_P);
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

      public static MethodWeavingInfo AddException(this MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForMethod> interceptorParameterConfigurations_P)
      {
         interceptorParameterConfigurations_P.AddPossibleParameter("exception")
            .WhichCanNotBeReferenced()
            .WhichMustBeOfType<VariablesForMethod, Exception>()
            .AndInjectTheVariable(variables => variables.Exception.Definition);
         return weavingInfo_P;
      }

      public static MethodWeavingInfo AddResult(this MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForMethod> interceptorParameterConfigurations_P)
      {
         interceptorParameterConfigurations_P.AddPossibleParameter("result")
            .WhereParameterTypeIsSameAsMethodResult(weavingInfo_P)
            .AndInjectTheVariable(variables => variables.Result.Definition);
         return weavingInfo_P;
      }

      public static InstructionWeavingInfo AddResult(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForInstruction> interceptorParameterConfigurations_P)
      {
         interceptorParameterConfigurations_P.AddPossibleParameter("result")
            .WhereParameterTypeIsSameAsMethodResultAndNotReferenced(weavingInfo_P)
            .AndInjectTheVariable(variables => variables.ResultForInstruction.Definition);
         return weavingInfo_P;
      }
   }
}
