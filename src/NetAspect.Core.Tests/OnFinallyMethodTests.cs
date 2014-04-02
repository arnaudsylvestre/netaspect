using System;
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
    public class AssertInstructions
    {
        private List<OpCode> expected = new List<OpCode>();

        public void Add(OpCode opcode)
        {
            expected.Add(opcode);
        }

        public void Check(MethodDefinition method)
        {
            try
            {
                var instructions = method.Body.Instructions;
                Assert.AreEqual(expected.Count, instructions.Count);
                for (int i = 0; i < instructions.Count; i++)
                {
                    Assert.AreEqual(expected[i], instructions[i].OpCode);
                }
            }
            catch (Exception)
            {
                foreach (var instruction in method.Body.Instructions)
                {
                    Console.WriteLine("assert.Add(OpCodes.{0});", instruction.OpCode.Code);
                }
                throw;
            }
            
        }
    }

    [TestFixture]
    public class OnFinallyMethodTests
    {
        [Test]
        public void CheckNopOnFinally()
        {
            var weavingModel = new NetAspectWeavingMethod()
            {
                OnFinallyInstructions = new List<Instruction> { Instruction.Create(OpCodes.Nop) }
            };
            var assert = new AssertInstructions();
            NetAspectCoreTestHelper.UpdateMethod(GetType(), "NopOnFinally", weavingModel, (o, info) => info.Invoke(o, new object[0]), assert);
        }

        


        public void NopOnFinally()
        {

        }

        [Test]
        public void CheckExceptionAndFinally()
        {
            var weavingModel = new NetAspectWeavingMethod()
            {
                OnFinallyInstructions = new List<Instruction> { Instruction.Create(OpCodes.Nop) },
                OnExceptionInstructions = new List<Instruction> { Instruction.Create(OpCodes.Nop) }
            };
            var assert = new AssertInstructions();
            NetAspectCoreTestHelper.UpdateMethod(GetType(), "ExceptionAndFinally", weavingModel, (o, info) => info.Invoke(o, new object[0]), assert);
        }



        public void ExceptionAndFinally()
        {
            
        }

        [Test]
        public void CheckFinallyWithReturn()
        {
            var weavingModel = new NetAspectWeavingMethod()
            {
                OnFinallyInstructions = new List<Instruction> { Instruction.Create(OpCodes.Nop) },
            };
            var assert = new AssertInstructions();
            NetAspectCoreTestHelper.UpdateMethod(GetType(), "FinallyWithReturn", weavingModel, (o, info) => info.Invoke(o, new object[0]), assert);
        }



        public string FinallyWithReturn()
        {
            return "value";
        }
    }
}
