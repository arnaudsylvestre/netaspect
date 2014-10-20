using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Weaver.Engine.InterceptorParameters;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Engine
{
    public static class CallerPossiblity
    {
        public static MethodWeavingInfo AddCaller(this MethodWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForInstruction> interceptorParameterPossibilitiesP)
        {
            return weavingInfo_P.AddCaller(interceptorParameterPossibilitiesP, "caller");
        }

        public static MethodWeavingInfo AddCallerMethod(this MethodWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForInstruction> interceptorParameterPossibilitiesP)
        {
            return weavingInfo_P.AddMethod(interceptorParameterPossibilitiesP, "callermethod");
        }
    }

    public static class ThisPossibility
    {
        public static MethodWeavingInfo AddInstance(this MethodWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForMethod> interceptorParameterPossibilitiesP)
        {
            return weavingInfo_P.AddCaller(interceptorParameterPossibilitiesP, "instance");
        }
    }

    public static class PdbPossibility
    {

        public static InstructionWeavingInfo AddColumnNumber(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForInstruction> interceptorParameterPossibilitiesP)
        {
            interceptorParameterPossibilitiesP.AddPossibleParameter("columnnumber")
               .WhichCanNotBeReferenced()
               .WhichPdbPresent(weavingInfo_P)
               .WhichMustBeOfType<VariablesForInstruction, int>()
               .AndInjectThePdbInfo(s => s.StartColumn, weavingInfo_P);
            return weavingInfo_P;
        }

        public static InstructionWeavingInfo AddLineNumber(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForInstruction> interceptorParameterPossibilitiesP)
        {
            interceptorParameterPossibilitiesP.AddPossibleParameter("linenumber")
               .WhichCanNotBeReferenced()
               .WhichPdbPresent(weavingInfo_P)
               .WhichMustBeOfType<VariablesForInstruction, int>()
               .AndInjectThePdbInfo(s => s.StartLine, weavingInfo_P);
            return weavingInfo_P;
        }
        
    }

    public static class ParameterPossibility
    {
        public static ParameterWeavingInfo AddParameterValue(this ParameterWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForMethod> interceptorParameterPossibilitiesP)
        {
            interceptorParameterPossibilitiesP.AddPossibleParameter("parametervalue")
               .WhichCanNotBeOut()
               .WhichMustBeOfTypeOfParameter(weavingInfo_P.Parameter)
               .AndInjectTheParameter(weavingInfo_P.Parameter);
            return weavingInfo_P;
        }

        public static ParameterWeavingInfo AddParameterName(this ParameterWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForMethod> interceptorParameterPossibilitiesP)
        {
            interceptorParameterPossibilitiesP.AddPossibleParameter("parametername")
               .WhichCanNotBeReferenced()
               .WhichMustBeOfType<VariablesForMethod, string>()
               .AndInjectTheValue(weavingInfo_P.Parameter.Name);
            return weavingInfo_P;
        }

        public static ParameterWeavingInfo AddParameterInfo(this ParameterWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForMethod> interceptorParameterPossibilitiesP)
        {
            interceptorParameterPossibilitiesP.AddPossibleParameter("parameter")
               .WhichCanNotBeReferenced()
               .WhichMustBeOfType<VariablesForMethod, ParameterInfo>()
               .AndInjectTheParameterInfo(weavingInfo_P.Parameter, weavingInfo_P.Method);
            return weavingInfo_P;
        }
    }

   public static class CalledPossibility
   {
       public static InstructionWeavingInfo AddCalled(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForInstruction> interceptorParameterPossibilitiesP, IMemberDefinition member)
      {
         interceptorParameterPossibilitiesP.AddPossibleParameter("called")
            .WhichCanNotBeReferenced()
            .WhereFieldCanNotBeStatic(member)
            .WhichMustBeOfTypeOf(member.DeclaringType)
            .AndInjectTheCalledInstance();
         return weavingInfo_P;
      }

       public static InstructionWeavingInfo AddCalledParameters(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForInstruction> interceptorParameterPossibilitiesP)
       {
           interceptorParameterPossibilitiesP.AddPossibleParameter("calledparameters")
              .WhichCanNotBeReferenced()
              .WhichMustBeOfType<VariablesForInstruction, object[]>()
              .AndInjectTheVariable(variables => variables.CalledParametersObjects.Definition);
           return weavingInfo_P;
       }

       public static InstructionWeavingInfo AddCalledParameterNames(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForInstruction> interceptorParameterPossibilitiesP)
       {
           foreach (ParameterDefinition parameterDefinition in weavingInfo_P.GetOperandAsMethod().Parameters)
           {
               interceptorParameterPossibilitiesP.AddPossibleParameter("called" + parameterDefinition.Name.ToLower())
                  .WhichCanNotBeReferenced()
                  .WhichMustBeOfTypeOfParameter(parameterDefinition)
                  .AndInjectTheCalledParameter(parameterDefinition);
           }
           return weavingInfo_P;
       }

       

       

       

      public static MethodWeavingInfo AddCaller<T>(this MethodWeavingInfo weavingInfo_P,
         InterceptorParameterPossibilities<T> interceptorParameterPossibilitiesP,
         string parameterName) where T : VariablesForMethod

      {
         interceptorParameterPossibilitiesP.AddPossibleParameter(parameterName)
            .WhichCanNotBeReferenced()
            .WhereCurrentMethodCanNotBeStatic(weavingInfo_P)
            .OrOfCurrentMethodDeclaringType(weavingInfo_P)
            .AndInjectTheCurrentInstance();
         return weavingInfo_P;
      }


      public static MethodWeavingInfo AddMethod<T>(this MethodWeavingInfo weavingInfo_P,
         InterceptorParameterPossibilities<T> interceptorParameterPossibilitiesP,
         string parameterName)
          where T : VariablesForMethod
      {
         interceptorParameterPossibilitiesP.AddPossibleParameter(parameterName)
            .WhichCanNotBeReferenced()
            .WhichMustBeOfType<T, MethodBase>()
            .AndInjectTheCurrentMethod();
         return weavingInfo_P;
      }

      public static MethodWeavingInfo AddProperty(this MethodWeavingInfo weavingInfo_P,
         InterceptorParameterPossibilities<VariablesForMethod> interceptorParameterPossibilitiesP)
      {
         interceptorParameterPossibilitiesP.AddPossibleParameter("property")
            .WhichCanNotBeReferenced()
            .WhichMustBeOfType<VariablesForMethod, PropertyInfo>()
            .AndInjectTheCurrentProperty();
         return weavingInfo_P;
      }

      public static MethodWeavingInfo AddMethod(this MethodWeavingInfo weavingInfo_P,
         InterceptorParameterPossibilities<VariablesForMethod> interceptorParameterPossibilitiesP)
      {
         return AddMethod(weavingInfo_P, interceptorParameterPossibilitiesP, "method");
      }

      public static MethodWeavingInfo AddConstructor(this MethodWeavingInfo weavingInfo_P,
         InterceptorParameterPossibilities<VariablesForMethod> interceptorParameterPossibilitiesP)
      {
         return AddMethod(weavingInfo_P, interceptorParameterPossibilitiesP, "constructor");
      }

      public static MethodWeavingInfo AddCallerParameters(this MethodWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForInstruction> interceptorParameterPossibilitiesP)
      {
         return AddCurrentParameters(weavingInfo_P, interceptorParameterPossibilitiesP, "callerparameters");
      }

      public static MethodWeavingInfo AddParameters(this MethodWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForMethod> interceptorParameterPossibilitiesP)
      {
         return AddCurrentParameters(weavingInfo_P, interceptorParameterPossibilitiesP, "parameters");
      }

      private static MethodWeavingInfo AddCurrentParameters<T>(MethodWeavingInfo weavingInfo_P,
         InterceptorParameterPossibilities<T>
            interceptorParameterPossibilitiesP,
         string parameterName) where T : VariablesForMethod
      {
         interceptorParameterPossibilitiesP.AddPossibleParameter(parameterName)
            .WhichCanNotBeReferenced()
            .WhichMustBeOfType<T, object[]>()
            .AndInjectTheVariable(variables => variables.Parameters.Definition);
         return weavingInfo_P;
      }

      public static MethodWeavingInfo AddCallerParameterNames(this MethodWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForInstruction> interceptorParameterPossibilitiesP)
      {
         return AddParameterNames(weavingInfo_P, interceptorParameterPossibilitiesP, () => "caller{0}");
      }

      public static MethodWeavingInfo AddParameterNames<T>(this MethodWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<T> interceptorParameterPossibilitiesP) where T : VariablesForMethod
      {
         return AddParameterNames(weavingInfo_P, interceptorParameterPossibilitiesP, () => "{0}");
      }

      private static MethodWeavingInfo AddParameterNames<T>(MethodWeavingInfo weavingInfo_P,
         InterceptorParameterPossibilities<T>
            interceptorParameterPossibilitiesP,
         Func<string> parameterNameFormatProvider)
          where T : VariablesForMethod
      {
         foreach (ParameterDefinition parameter in weavingInfo_P.Method.Parameters)
         {
            interceptorParameterPossibilitiesP.AddPossibleParameter(
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
         InterceptorParameterPossibilities<VariablesForMethod>
            interceptorParameterPossibilitiesP)
      {
         ParameterDefinition parameter = weavingInfo_P.Method.Parameters[0];
         interceptorParameterPossibilitiesP.AddPossibleParameter("value")
            .WhichCanNotBeOut()
            .WhichMustBeOfTypeOfParameter(parameter)
            .AndInjectTheParameter(parameter);
         return weavingInfo_P;
      }

      public static MethodWeavingInfo AddPropertyValueForInstruction(this InstructionWeavingInfo weavingInfo_P,
         InterceptorParameterPossibilities<VariablesForInstruction>
            interceptorParameterPossibilitiesP)
      {
         ParameterDefinition parameter = weavingInfo_P.GetOperandAsMethod().Parameters.Last();
         interceptorParameterPossibilitiesP.AddPossibleParameter("propertyvalue")
            .WhichCanNotBeReferenced()
            .WhichMustBeOfTypeOfParameter(parameter)
            .AndInjectTheCalledValue(parameter);
         return weavingInfo_P;
      }

      public static MethodWeavingInfo AddFieldValue(this InstructionWeavingInfo weavingInfo_P,
         InterceptorParameterPossibilities<VariablesForInstruction>
            interceptorParameterPossibilitiesP)
      {
         FieldDefinition field = weavingInfo_P.GetOperandAsField();
         TypeReference parameter = field.FieldType;
         interceptorParameterPossibilitiesP.AddPossibleParameter("fieldvalue")
            .WhichCanNotBeReferenced()
            .WhichMustBeOfTypeOf(parameter)
            .AndInjectTheFieldValue(field);
         return weavingInfo_P;
      }

      public static MethodWeavingInfo AddLineNumberForMethod(this MethodWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForMethod> interceptorParameterPossibilitiesP)
      {
          interceptorParameterPossibilitiesP.AddPossibleParameter("linenumber")
             .WhichCanNotBeReferenced()
             .WhichPdbPresentForMethod(weavingInfo_P)
             .WhichMustBeOfType<VariablesForMethod, int>()
             .AndInjectThePdbInfoForMethod(s => s.StartLine, weavingInfo_P);
          return weavingInfo_P;
      }
      public static MethodWeavingInfo AddColumnNumberForMethod(this MethodWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForMethod> interceptorParameterPossibilitiesP)
      {
          interceptorParameterPossibilitiesP.AddPossibleParameter("columnnumber")
             .WhichCanNotBeReferenced()
             .WhichPdbPresentForMethod(weavingInfo_P)
             .WhichMustBeOfType<VariablesForMethod, int>()
             .AndInjectThePdbInfoForMethod(s => s.StartColumn, weavingInfo_P);
          return weavingInfo_P;
      }

      public static InstructionWeavingInfo AddFilePath(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForInstruction> interceptorParameterPossibilitiesP)
      {
         interceptorParameterPossibilitiesP.AddPossibleParameter("filepath")
            .WhichCanNotBeReferenced()
            .WhichPdbPresent(weavingInfo_P)
            .WhichMustBeOfType<VariablesForInstruction, string>()
            .AndInjectThePdbInfo(s => s.Document.Url, weavingInfo_P);
         return weavingInfo_P;
      }

      public static MethodWeavingInfo AddFilePathForMethod(this MethodWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForMethod> interceptorParameterPossibilitiesP)
      {
         interceptorParameterPossibilitiesP.AddPossibleParameter("filepath")
            .WhichCanNotBeReferenced()
            .WhichPdbPresentForMethod(weavingInfo_P)
            .WhichMustBeOfType<VariablesForMethod, string>()
            .AndInjectThePdbInfoForMethod(s => s.Document.Url, weavingInfo_P);
         return weavingInfo_P;
      }

      public static InstructionWeavingInfo AddFileName(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForInstruction> interceptorParameterPossibilitiesP)
      {
         interceptorParameterPossibilitiesP.AddPossibleParameter("filename")
            .WhichCanNotBeReferenced()
            .WhichPdbPresent(weavingInfo_P)
            .WhichMustBeOfType<VariablesForInstruction, string>()
            .AndInjectThePdbInfo(s => Path.GetFileName(s.Document.Url), weavingInfo_P);
         return weavingInfo_P;
      }

      public static MethodWeavingInfo AddFileNameForMethod(this MethodWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForMethod> interceptorParameterPossibilitiesP)
      {
         interceptorParameterPossibilitiesP.AddPossibleParameter("filename")
            .WhichCanNotBeReferenced()
            .WhichPdbPresentForMethod(weavingInfo_P)
            .WhichMustBeOfType<VariablesForMethod, string>()
            .AndInjectThePdbInfoForMethod(s => Path.GetFileName(s.Document.Url), weavingInfo_P);
         return weavingInfo_P;
      }

      public static InstructionWeavingInfo AddCalledFieldInfo(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForInstruction> interceptorParameterPossibilitiesP)
      {
         interceptorParameterPossibilitiesP.AddPossibleParameter("field")
            .WhichCanNotBeReferenced()
            .WhichMustBeOfType<VariablesForInstruction, FieldInfo>()
            .AndInjectTheCalledFieldInfo(weavingInfo_P);
         return weavingInfo_P;
      }

      public static InstructionWeavingInfo AddCalledPropertyInfo(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForInstruction> interceptorParameterPossibilitiesP)
      {
         interceptorParameterPossibilitiesP.AddPossibleParameter("property")
            .WhichCanNotBeReferenced()
            .WhichMustBeOfType<VariablesForInstruction, PropertyInfo>()
            .AndInjectTheCalledPropertyInfo(weavingInfo_P);
         return weavingInfo_P;
      }

      public static MethodWeavingInfo AddException(this MethodWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForMethod> interceptorParameterPossibilitiesP)
      {
         interceptorParameterPossibilitiesP.AddPossibleParameter("exception")
            .WhichCanNotBeReferenced()
            .WhichMustBeOfType<VariablesForMethod, Exception>()
            .AndInjectTheVariable(variables => variables.Exception.Definition);
         return weavingInfo_P;
      }

      public static MethodWeavingInfo AddResult(this MethodWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForMethod> interceptorParameterPossibilitiesP)
      {
         interceptorParameterPossibilitiesP.AddPossibleParameter("result")
            .WhereParameterTypeIsSameAsMethodResult(weavingInfo_P)
            .AndInjectTheVariable(variables => variables.Result.Definition);
         return weavingInfo_P;
      }

      public static InstructionWeavingInfo AddResult(this InstructionWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForInstruction> interceptorParameterPossibilitiesP)
      {
         interceptorParameterPossibilitiesP.AddPossibleParameter("result")
            .WhereParameterTypeIsSameAsMethodResultAndNotReferenced(weavingInfo_P)
            .AndInjectTheVariable(variables => variables.ResultForInstruction.Definition);
         return weavingInfo_P;
      }
   }
}
