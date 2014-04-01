﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FluentAspect.Weaver.Apis.AssemblyChecker.Peverify;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NUnit.Framework;

namespace NetAspect.Core.Tests
{
    [TestFixture]
    public class BeforeMethodTests
    {
        [Test]
        public void CheckBeforeMethod()
        {
            var weavingModel = new NetAspectWeavingMethod()
            {
                OnFinallyInstructions = new List<Instruction> { Instruction.Create(OpCodes.Nop) }
            };
            UpdateMethod(GetType(), "EmptyMethod", weavingModel, (o, info) => info.Invoke(o, new object[0]));
        }

        private static void UpdateMethod(Type type, string methodName, NetAspectWeavingMethod weavingModel, Action<object, MethodInfo> callWeavedMethod)
        {
            var assembly = AssemblyDefinition.ReadAssembly("NetAspect.Core.Tests.dll",
                                                           new ReaderParameters() {ReadSymbols = true});

            assembly.MainModule.GetType(type.FullName).Methods.First(m => m.Name == methodName).Weave(weavingModel);
            var newAssemblyName = "NetAspect.Core.Tests.Weaved.dll";
            assembly.Write(newAssemblyName, new WriterParameters() {WriteSymbols = true});
            ProcessHelper.Launch("peverify.exe", "\"" + Path.GetFullPath(newAssemblyName) + "\"");
            var weavedType = Assembly.LoadFrom(newAssemblyName).GetType(type.FullName);
            var instance = Activator.CreateInstance(weavedType);
            var methodInfo = weavedType.GetMethod(methodName);
            callWeavedMethod(instance, methodInfo);
        }

        public void EmptyMethod()
        {
            
        }
    }
}
