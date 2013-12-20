using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Helpers;
using FluentAspect.Weaver.Weavers.Helpers;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MethodAttributes = Mono.Cecil.MethodAttributes;

namespace FluentAspect.Weaver.Weavers
{
   public class MethodAroundWeaver
   {
      public void CreateWeaver(MethodDefinition method, Type interceptorType, MethodDefinition wrappedMethod)
      {
         CreateWeaver(new Method(method), interceptorType, wrappedMethod);
      }

      public void CreateWeaver(Method myMethod, Type interceptorType, MethodDefinition wrappedMethod)
      {
         var interceptor = myMethod.CreateAndInitializeVariable(interceptorType);
         var args = myMethod.CreateArgsArrayFromParameters();
         var methodInfo = myMethod.CreateMethodInfo();
         var weavedResult = myMethod.CreateWeavedResult();

          myMethod.Add(new TryCatch(
                           il => {
                                     CallBefore(myMethod.MethodDefinition, interceptor, methodInfo, args, interceptorType, il);
                                     var result = CallWeavedMethod(myMethod.MethodDefinition, wrappedMethod, il);
                                     var handleResult = myMethod.CreateHandleResult(result);
                                     CallAfter(myMethod.MethodDefinition, interceptor, methodInfo, args, handleResult, interceptorType, il);
                                     SetReturnValue(myMethod.MethodDefinition, handleResult, weavedResult, il);
                           },
                           il => {
                                     var e = CreateException(myMethod.MethodDefinition);
                                     CallExceptionInterceptor(myMethod.MethodDefinition, interceptor, methodInfo, args, e, interceptorType, myMethod.Il);
                                     myMethod.Il.AppendThrow();
                           }));


          myMethod.Return(weavedResult);

      }


      

       

      private void CallExceptionInterceptor(MethodDefinition method,
                                            VariableDefinition interceptor,
                                            VariableDefinition methodInfo,
                                            VariableDefinition args,
                                            VariableDefinition ex,
                                            Type interceptorType,
                                            ILProcessor il)
      {
         il.Emit(OpCodes.Stloc, ex);
         il.Emit(OpCodes.Ldloc, interceptor);

         var forParameters = new Dictionary<string, Action>();
         forParameters.Add("instance", () => il.Emit(OpCodes.Ldarg_0));
         forParameters.Add("method", () => il.Emit(OpCodes.Ldloc, methodInfo));
         forParameters.Add("parameters", () => il.Emit(OpCodes.Ldloc, args));
         forParameters.Add("exception", () => il.Emit(OpCodes.Ldloc, ex));

         var beforeMethod = interceptorType.GetMethod("OnException");
         if (beforeMethod == null)
             return;
         il.Emit(OpCodes.Ldloc, interceptor);

         foreach (var parameterInfo in beforeMethod.GetParameters())
         {
             forParameters[parameterInfo.Name]();
         }
         il.Emit(OpCodes.Call, method.Module.Import(beforeMethod));
      }

      private VariableDefinition CreateException(MethodDefinition method)
      {
          return method.CreateVariable(typeof(Exception));
      }

      private void SetReturnValue(MethodDefinition method,
                                  VariableDefinition handleResult,
                                  VariableDefinition weavedResult,
                                  ILProcessor il)
      {
         if (method.ReturnType.MetadataType != MetadataType.Void)
         {
            il.Emit(OpCodes.Ldloc, handleResult);
            il.Emit(OpCodes.Castclass, method.ReturnType);
            il.Emit(OpCodes.Stloc, weavedResult);
         }
      }

      private void CallAfter(MethodDefinition method,
                             VariableDefinition interceptor,
                             VariableDefinition methodInfo,
                             VariableDefinition args,
                             VariableDefinition handleResult,
                             Type interceptorType,
                             ILProcessor il)
      {
          var forParameters = new Dictionary<string, Action<ParameterInfo>>();
          forParameters.Add("instance", (p) => il.Emit(OpCodes.Ldarg_0));
          forParameters.Add("method", (p) => il.Emit(OpCodes.Ldloc, methodInfo));
          forParameters.Add("parameters", (p) => 
              il.Emit(OpCodes.Ldloc, args));
          forParameters.Add("result", (p) => 
              il.Emit(p.ParameterType.IsByRef ? OpCodes.Ldloca : OpCodes.Ldloc, handleResult));


          var afterMethod = interceptorType.GetMethod("After");
          if (afterMethod == null)
              return;

          il.Emit(OpCodes.Ldloc, interceptor);

          foreach (var parameterInfo in afterMethod.GetParameters())
          {
              forParameters[parameterInfo.Name](parameterInfo);
          }
          il.Emit(OpCodes.Call, method.Module.Import(afterMethod));
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
                              VariableDefinition interceptor,
                              VariableDefinition methodInfo,
                              VariableDefinition args,
                              Type interceptorType,
                              ILProcessor il)
      {
          var forParameters = new Dictionary<string, Action>();
          forParameters.Add("instance", () => il.Emit(OpCodes.Ldarg_0));
          forParameters.Add("method", () => il.Emit(OpCodes.Ldloc, methodInfo));
          forParameters.Add("parameters", () => il.Emit(OpCodes.Ldloc, args));

          var beforeMethod = interceptorType.GetMethod("Before");
          if (beforeMethod == null)
              return;
          il.Emit(OpCodes.Ldloc, interceptor);

          foreach (var parameterInfo in beforeMethod.GetParameters())
          {
              forParameters[parameterInfo.Name]();
          }
          il.Emit(OpCodes.Call, method.Module.Import(beforeMethod));
      }
   }
}
