using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAspect.Core;
using FluentAspect.Core.Expressions;
using FluentAspect.Core.Methods;
using FluentAspect.Weaver.Helpers;
using FluentAspect.Weaver.Weavers;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MethodAttributes = System.Reflection.MethodAttributes;

namespace FluentAspect.Weaver
{
    class TypeDefinitionAdapter : IType
    {
        private TypeDefinition type;

        public TypeDefinitionAdapter(TypeDefinition type_P)
        {
            type = type_P;
        }

        public string Name
        {
            get { return type.Name; }
        }
    }

    class MethodDefinitionAdapter : IMethod
    {
        private MethodDefinition method;

        public MethodDefinitionAdapter(MethodDefinition method_P)
        {
            method = method_P;
        }

        public string Name
        {
            get { return method.Name; }
        }
        public IType DeclaringType
        {
            get { return new TypeDefinitionAdapter(method.DeclaringType); }
        }
    }

   public class WeaverTool
   {
       private readonly string _asm;
      private readonly string _dest;

      public WeaverTool(string asm)
         :this(asm, asm)
       {
          _asm = asm;
       }
       public WeaverTool(string asm, string dest_P)
       {
          _asm = asm;
          _dest = dest_P;
       }

      public void Weave()
      {
         var definition = Assembly.LoadFrom(_asm);
         
         foreach (var typeDefinition_L in definition.GetTypes())
         {
            if (typeDefinition_L.BaseType == typeof(FluentAspectDefinition))
            {
               var instance_L = (FluentAspectDefinition) Activator.CreateInstance(typeDefinition_L);
               instance_L.Setup();
               var methodMatches_L = instance_L.methodMatches;
                var assemblyDefinition = AssemblyDefinition.ReadAssembly(_asm, new ReaderParameters(ReadingMode.Immediate) {ReadSymbols = true});

                foreach (var moduleDefinition in assemblyDefinition.Modules)
                {
                    foreach (var typeDefinition in moduleDefinition.GetTypes())
                    {
                        var toAdd = new List<MethodDefinition>();
                        foreach (var methodDefinition in typeDefinition.Methods)
                        {
                            var methodDefinitionAdapter = new MethodDefinitionAdapter(methodDefinition);
                            foreach (var methodMatch_L in methodMatches_L)
                            {
                                if (methodMatch_L.Matcher(methodDefinitionAdapter))
                                {
                                    var interceptor = methodMatch_L.AdviceName;
                                    toAdd.Add(Around(methodDefinition, interceptor, moduleDefinition));
                                }
                            }
                        }
                        foreach (var methodDefinition1 in toAdd)
                        {
                            typeDefinition.Methods.Add(methodDefinition1);
                        }
                    }
                }
                var moduleDefinitions = assemblyDefinition.Modules;
                foreach (var moduleDefinition in moduleDefinitions)
                {
                    var same = (from r in moduleDefinition.AssemblyReferences where r.FullName == assemblyDefinition.FullName select r).ToList();
                    foreach (var reference in same)
                    {
                        moduleDefinition.AssemblyReferences.Remove(reference);
                    }
                }
                assemblyDefinition.Write(_dest, new WriterParameters()
                    {
                        WriteSymbols = true,
                    });
            }
         }
      }

       private MethodDefinition Around(MethodDefinition methodDefinition, Type interceptor, ModuleDefinition moduleDefinition)
       {
           var weavedMethodName = ComputeNewName(methodDefinition);
           var definition = CreateNewMethodBasedOnMethodToWeave(methodDefinition, weavedMethodName, interceptor);
           return definition;
       }

       private MethodDefinition CreateNewMethodBasedOnMethodToWeave(MethodDefinition methodDefinition, string weavedMethodName, Type interceptor)
       {
           MethodDefinition wrappedMethod = new MethodDefinition(ComputeNewName(methodDefinition), methodDefinition.Attributes, methodDefinition.ReturnType);

           foreach (var parameterDefinition in methodDefinition.Parameters)
           {
               wrappedMethod.Parameters.Add(parameterDefinition);
           }
           foreach (var instruction in methodDefinition.Body.Instructions)
           {
               wrappedMethod.Body.Instructions.Add(instruction);
           }
           methodDefinition.Body.Instructions.Clear();
           foreach (var variable in methodDefinition.Body.Variables)
           {
               wrappedMethod.Body.Variables.Add(variable);
           }
           methodDefinition.Body.Variables.Clear();
           foreach (var exceptionHandler in methodDefinition.Body.ExceptionHandlers)
           {
               wrappedMethod.Body.ExceptionHandlers.Add(exceptionHandler);
           }
           foreach (var genericParameter in methodDefinition.GenericParameters)
           {
               wrappedMethod.GenericParameters.Add(new GenericParameter(genericParameter.Name, wrappedMethod));
           }
           methodDefinition.Body.InitLocals = true;
           MethodAroundWeaver weaver = new MethodAroundWeaver();
           weaver.CreateWeaver(methodDefinition, interceptor, wrappedMethod);
           //var wrapperMethod = new MethodDefinition(methodDefinition.Name, methodDefinition.Attributes, methodDefinition.ReturnType) ;
           //methodDefinition.Name = weavedMethodName;
           //methodDefinition.GenericParameters.TransferItemsTo(wrapperMethod.GenericParameters);
           //foreach (var p in wrapperMethod.GenericParameters)
           //{
           //    p.GetType().GetField("owner", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(p, wrapperMethod);
           //    methodDefinition.GenericParameters.Add(new GenericParameter(p.Name, methodDefinition));
           //}
           //var il = wrapperMethod.Body.GetILProcessor();

           //var variableDefinition = new VariableDefinition(methodDefinition.Module.Import(typeof(object[])));
           //wrapperMethod.Body.Variables.Add(variableDefinition);
           //var args = variableDefinition;

           //il.Append(il.Create(OpCodes.Ldc_I4, methodDefinition.Parameters.Count));
           //il.Append(il.Create(OpCodes.Newarr, methodDefinition.Module.Import(typeof(object))));
           //il.Append(il.Create(OpCodes.Stloc, args));

           //var targetParams = methodDefinition.Parameters.ToArray();
           //foreach (var p in targetParams)
           //{
           //    wrapperMethod.Parameters.Add(p);

           //    il.Append(il.Create(OpCodes.Ldloc, args));
           //    il.Append(il.Create(OpCodes.Ldc_I4, p.Index));
           //    il.Append(il.Create(OpCodes.Ldarg, p));
           //    if (p.ParameterType.IsValueType)
           //        il.Append(il.Create(OpCodes.Box, p.ParameterType));
           //    il.Append(il.Create(OpCodes.Stelem_Ref));
           //}

           ////wrapperMethod.Body.InitLocals = methodDefinition.Body.InitLocals;

           ////il.Append(il.Create(OpCodes.Ldarg_0));
           ////il.Append(il.Create(OpCodes.Ldstr, weavedMethodName));
           ////il.Append(il.Create(OpCodes.Ldloc, args));
           ////il.Append(il.Create(OpCodes.Newobj, moduleDefinition.Import(interceptor.GetConstructors()[0])));
           ////il.Append(il.Create(OpCodes.Call, moduleDefinition.Import(typeof(Around).GetMethod("Call"))));
           ////il.Append(il.Create(OpCodes.Pop));
           ////var ret = il.Create(OpCodes.Ret);
           ////var leave = il.Create(OpCodes.Leave, ret);
           //if (methodDefinition.ReturnType.MetadataType == MetadataType.Void)
           // il.Append(il.Create(OpCodes.Pop));
           ////il.Emit(OpCodes.Nop);
           //il.Append(il.Create(OpCodes.Ret));



           return wrappedMethod;
       }

       private string ComputeNewName(MethodDefinition methodDefinition)
       {
           return methodDefinition.Name + "<>Weaved<>";
       }
   }
}