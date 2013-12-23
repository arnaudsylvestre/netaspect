using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAspect.Weaver.Helpers;
using FluentAspect.Weaver.Weavers.Helpers;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MethodAttributes = Mono.Cecil.MethodAttributes;

namespace FluentAspect.Weaver.Weavers
{


    public class MethodMethodAroundWeaverConfiguration : MethodAroundWeaver.IAroundWeaverConfiguration
    {
        public MethodInfo ToCallBefore(Type interceptorType)
        {
            return interceptorType.GetMethod("Before");
        }

        public MethodInfo ToCallAfter(Type interceptorType)
        {
            return interceptorType.GetMethod("After");
        }

        public MethodInfo ToCallOnException(Type interceptorType)
        {
            return interceptorType.GetMethod("OnException");
        }
    }

    public static class AroundWeaverConfigurationExtensions
    {
        public static bool Needs(this MethodAroundWeaver.IAroundWeaverConfiguration configuration, List<Type> interceptorTypes, string variableName)
        {
            foreach (var interceptorType in interceptorTypes)
            {
                var parameters = new List<ParameterInfo>();
                var callBefore = configuration.ToCallBefore(interceptorType);
                if (callBefore != null)
                parameters.AddRange(callBefore.GetParameters().ToList());
                var methodInfo = configuration.ToCallAfter(interceptorType);
                if (methodInfo != null)
                parameters.AddRange(methodInfo.GetParameters().ToList());
                var callOnException = configuration.ToCallOnException(interceptorType);
                if (callOnException != null)
                    parameters.AddRange(callOnException.GetParameters().ToList());

                var enumerable = from p in parameters where p.Name == variableName select p.Name;
                if (enumerable.Any())
                    return true;
            }
            return false;
        }

        public static bool NeedsCallBefore(this MethodAroundWeaver.IAroundWeaverConfiguration configuration,
                                           List<Type> interceptorTypes)
        {
            return interceptorTypes.Any(interceptorType => configuration.ToCallBefore(interceptorType) != null);
        }

        public static bool NeedsCallAfter(this MethodAroundWeaver.IAroundWeaverConfiguration configuration,
                                           List<Type> interceptorTypes)
        {
            return interceptorTypes.Any(interceptorType => configuration.ToCallAfter(interceptorType) != null);
        }

        public static bool NeedsCallOnException(this MethodAroundWeaver.IAroundWeaverConfiguration configuration,
                                           List<Type> interceptorTypes)
        {
            return interceptorTypes.Any(interceptorType => configuration.ToCallOnException(interceptorType) != null);
        }
    }

   public class MethodAroundWeaver
   {
       private const string ParameterParameters = "parameters";
       private const string Instance = "instance";

       public interface IAroundWeaverConfiguration
       {
           MethodInfo ToCallBefore(Type interceptorType);
           MethodInfo ToCallAfter(Type interceptorType);
           MethodInfo ToCallOnException(Type interceptorType);
       }

      public void CreateWeaver(MethodDefinition method, List<Type> interceptorType, MethodDefinition wrappedMethod, IAroundWeaverConfiguration configuration)
      {
          CreateWeaver(new Method(method), interceptorType, wrappedMethod, configuration);
      }

      public void CreateWeaver(Method myMethod, List<Type> interceptorType, MethodDefinition wrappedMethod, IAroundWeaverConfiguration configuration)
      {
         var interceptor = myMethod.CreateAndInitializeVariable(interceptorType);
         VariableDefinition args = null;
         if (configuration.Needs(interceptorType, ParameterParameters))
              args = myMethod.CreateArgsArrayFromParameters();
         var methodInfo = myMethod.CreateMethodInfo();
         var weavedResult = myMethod.CreateWeavedResult();

          myMethod.Add(new TryCatch(
                           il => {
                                     CallBefore(myMethod.MethodDefinition, interceptor, methodInfo, args, interceptorType, il);
                                     var result = CallWeavedMethod(myMethod.MethodDefinition, wrappedMethod, il);
                                     var handleResult = myMethod.CreateHandleResult(result);
                                     CallAfter(myMethod.MethodDefinition, interceptor, methodInfo, args, handleResult, interceptorType, il);
                                     myMethod.SetReturnValue(handleResult, weavedResult);
                           },
                           il => {
                                     var e = CreateException(myMethod.MethodDefinition);
                                     CallExceptionInterceptor(myMethod.MethodDefinition, interceptor, methodInfo, args, e, interceptorType, myMethod.Il);
                                     myMethod.Il.AppendThrow();
                           }));


          myMethod.Return(weavedResult);

      }

       public class InterceptorCaller
       {
            
       }

      private void CallInterceptor(MethodDefinition method)
       {
           
       }
       

      private void CallExceptionInterceptor(MethodDefinition method,
                                            List<VariableDefinition> interceptor,
                                            VariableDefinition methodInfo,
                                            VariableDefinition args,
                                            VariableDefinition ex,
                                            List<Type> interceptorType,
                                            ILProcessor il)
      {

          for (int i = 0; i < interceptorType.Count; i++)
          {
              var iType = interceptorType[i];

              il.Emit(OpCodes.Stloc, ex);

              var forParameters = new Dictionary<string, Action<ParameterInfo>>();
              forParameters.Add(Instance, (p) => il.Emit(OpCodes.Ldarg_0));
              forParameters.Add("method", (p) => il.Emit(OpCodes.Ldloc, methodInfo));
              forParameters.Add(ParameterParameters, (p) => il.Emit(OpCodes.Ldloc, args));
              forParameters.Add("exception", (p) => il.Emit(OpCodes.Ldloc, ex));


              foreach (var parameterDefinition in method.Parameters)
              {
                  ParameterDefinition definition = parameterDefinition;
                  forParameters.Add(parameterDefinition.Name, (p) =>
                  il.Emit(p.ParameterType.IsByRef ? OpCodes.Ldarga : OpCodes.Ldarg, definition));
              }

              var beforeMethod = iType.GetMethod("OnException");
              if (beforeMethod == null)
                  return;
              il.Emit(OpCodes.Ldloc, interceptor[i]);

              foreach (var parameterInfo in beforeMethod.GetParameters())
              {
                  forParameters[parameterInfo.Name](parameterInfo);
              }
              il.Emit(OpCodes.Call, method.Module.Import(beforeMethod));

          }
         
      }

      private VariableDefinition CreateException(MethodDefinition method)
      {
          return method.CreateVariable(typeof(Exception));
      }

      private void CallAfter(MethodDefinition method,
                             List<VariableDefinition> interceptor,
                             VariableDefinition methodInfo,
                             VariableDefinition args,
                             VariableDefinition handleResult,
                             List<Type> interceptorType,
                             ILProcessor il)
      {
          for (int i = 0; i < interceptorType.Count; i++)
          {
              var iType = interceptorType[i];

              var forParameters = new Dictionary<string, Action<ParameterInfo>>();
              forParameters.Add("instance", (p) => il.Emit(OpCodes.Ldarg_0));
              forParameters.Add("method", (p) => il.Emit(OpCodes.Ldloc, methodInfo));
              forParameters.Add(ParameterParameters, (p) =>
                  il.Emit(OpCodes.Ldloc, args));
              forParameters.Add("result", (p) =>
                  il.Emit(p.ParameterType.IsByRef ? OpCodes.Ldloca : OpCodes.Ldloc, handleResult));

              foreach (var parameterDefinition in method.Parameters)
              {
                  ParameterDefinition definition = parameterDefinition;
                  forParameters.Add(parameterDefinition.Name, (p) =>
                  il.Emit(p.ParameterType.IsByRef ? OpCodes.Ldarga : OpCodes.Ldarg, definition));
              }

              var afterMethod = iType.GetMethod("After");
              if (afterMethod == null)
                  return;

              il.Emit(OpCodes.Ldloc, interceptor[i]);

              foreach (var parameterInfo in afterMethod.GetParameters())
              {
                  forParameters[parameterInfo.Name](parameterInfo);
              }
              il.Emit(OpCodes.Call, method.Module.Import(afterMethod));
          }

          
      }

      private VariableDefinition CallWeavedMethod(MethodDefinition method,
                                                  MethodDefinition wrappedMethod,
                                                  ILProcessor il)
      {
         VariableDefinition result = null;
         if (method.ReturnType.MetadataType != MetadataType.Void)
         {
            result = new VariableDefinition("result", method.ReturnType);
            method.Body.Variables.Add(result);
         }
         il.Emit(OpCodes.Ldarg_0);
         foreach (ParameterDefinition p in method.Parameters.ToArray())
         {
            il.Emit(OpCodes.Ldarg, p);
         }

         OpCode call = OpCodes.Callvirt;
         if ((method.Attributes & MethodAttributes.Static) == MethodAttributes.Static)
            call = OpCodes.Call;
         il.Emit(call, wrappedMethod.MakeGeneric(method.GenericParameters.ToArray()));
         if (result != null)
            il.Emit(OpCodes.Stloc, result);

         return result;
      }

      private void CallBefore(MethodDefinition method,
                              List<VariableDefinition> interceptor,
                              VariableDefinition methodInfo,
                              VariableDefinition args,
                              List<Type> interceptorType,
                              ILProcessor il)
      {
          for (int i = 0; i < interceptorType.Count; i++)
          {
              var iType = interceptorType[i];

              var forParameters = new Dictionary<string, Action<ParameterInfo>>();
              forParameters.Add("instance", (p) => il.Emit(OpCodes.Ldarg_0));
              forParameters.Add("method", (p) => il.Emit(OpCodes.Ldloc, methodInfo));
              forParameters.Add(ParameterParameters, (p) => il.Emit(OpCodes.Ldloc, args));


              foreach (var parameterDefinition in method.Parameters)
              {
                  ParameterDefinition definition = parameterDefinition;
                  forParameters.Add(parameterDefinition.Name, (p) =>
                  il.Emit(p.ParameterType.IsByRef ? OpCodes.Ldarga : OpCodes.Ldarg, definition));
              }

              var beforeMethod = iType.GetMethod("Before");
              if (beforeMethod == null)
                  return;
              il.Emit(OpCodes.Ldloc, interceptor[i]);

              foreach (var parameterInfo in beforeMethod.GetParameters())
              {
                  if (!forParameters.ContainsKey(parameterInfo.Name))
                      throw new Exception(string.Format("Parameter {0} not recognized in interceptor {1}.{2} for method {3} in {4}", parameterInfo.Name, iType.Name, beforeMethod.Name, method.Name, method.DeclaringType.Name));
                  forParameters[parameterInfo.Name](parameterInfo);
              }
              il.Emit(OpCodes.Call, method.Module.Import(beforeMethod));

          }

          
      }
   }
}
