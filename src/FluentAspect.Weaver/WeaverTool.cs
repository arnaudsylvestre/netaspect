using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAspect.Core;
using FluentAspect.Core.Expressions;
using FluentAspect.Core.Methods;
using FluentAspect.Weaver.Helpers;
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

       public WeaverTool(string asm)
       {
           _asm = asm;
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
                var assemblyDefinition = AssemblyDefinition.ReadAssembly(_asm);

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
                                    var methodToCall = instance_L.GetType().GetMethod(methodMatch_L.AdviceName);
                                    toAdd.Add(Around(methodDefinition, methodToCall, moduleDefinition));
                                }
                            }
                        }
                        foreach (var methodDefinition1 in toAdd)
                        {
                            typeDefinition.Methods.Add(methodDefinition1);
                        }
                    }
                }

                assemblyDefinition.Write(_asm);
            }
         }
      }

       private MethodDefinition Around(MethodDefinition methodDefinition, MethodInfo methodToCall, ModuleDefinition moduleDefinition)
       {
           Check(methodToCall);
           var weavedMethodName = ComputeNewName(methodDefinition);
           var definition = CreateNewMethodBasedOnMethodToWeave(methodDefinition, moduleDefinition, weavedMethodName);
           return definition;
       }

       private MethodDefinition CreateNewMethodBasedOnMethodToWeave(MethodDefinition methodDefinition, ModuleDefinition moduleDefinition, string weavedMethodName)
       {
           var wrapperMethod = new MethodDefinition(methodDefinition.Name, methodDefinition.Attributes, methodDefinition.ReturnType) { IsPrivate = true };
           methodDefinition.Name = weavedMethodName;
           methodDefinition.GenericParameters.TransferItemsTo(wrapperMethod.GenericParameters);
           foreach (var p in wrapperMethod.GenericParameters)
           {
               p.GetType().GetField("owner", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(p, wrapperMethod);
               methodDefinition.GenericParameters.Add(new GenericParameter(p.Name, methodDefinition));
           }
           var il = wrapperMethod.Body.GetILProcessor();
           //il.Append(il.Create(OpCodes.Nop));
           il.Append(il.Create(OpCodes.Ldarg_0));
           il.Append(il.Create(OpCodes.Call, methodDefinition));

           moduleDefinition.Import(typeof(Around).GetMethod("Call"))

           return wrapperMethod;
       }

       private string ComputeNewName(MethodDefinition methodDefinition)
       {
           return methodDefinition.Name + "<>Weaved<>";
       }

       private void Check(MethodInfo methodToCall)
       {
           if ((methodToCall.Attributes & MethodAttributes.Static) != MethodAttributes.Static)
               throw new Exception("Advice must be static");
       }
   }
}