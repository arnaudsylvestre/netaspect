using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using NUnit.Framework;

namespace NetAspect.Core.Tests
{
   public class AssertInstructions
   {
      private readonly List<OpCode> expected = new List<OpCode>();

      public void Add(OpCode opcode)
      {
         expected.Add(opcode);
      }

      public void Check(MethodDefinition method)
      {
         try
         {
            Collection<Instruction> instructions = method.Body.Instructions;
            Assert.AreEqual(expected.Count, instructions.Count);
            for (int i = 0; i < instructions.Count; i++)
            {
               Assert.AreEqual(expected[i], instructions[i].OpCode);
            }
         }
         catch (Exception)
         {
            foreach (Instruction instruction in method.Body.Instructions)
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
      public void NopOnFinally()
      {
      }


      public void ExceptionAndFinally()
      {
      }


      public string FinallyWithReturn()
      {
         return "value";
      }

      [Test]
      public void CheckExceptionAndFinally()
      {
         var weavingModel = new NetAspectWeavingMethod
         {
            OnFinallyInstructions = new List<Instruction> {Instruction.Create(OpCodes.Nop)},
            OnExceptionInstructions = new List<Instruction> {Instruction.Create(OpCodes.Nop)}
         };
         var assert = new AssertInstructions();
         assert.Add(OpCodes.Nop);
         assert.Add(OpCodes.Leave);
         assert.Add(OpCodes.Nop);
         assert.Add(OpCodes.Leave);
         assert.Add(OpCodes.Nop);
         assert.Add(OpCodes.Nop);
         assert.Add(OpCodes.Leave);
         assert.Add(OpCodes.Nop);
         assert.Add(OpCodes.Endfinally);
         assert.Add(OpCodes.Ret);
         NetAspectCoreTestHelper.UpdateMethod(GetType(), "ExceptionAndFinally", weavingModel, (o, info) => info.Invoke(o, new object[0]), assert);
      }

      [Test]
      public void CheckFinallyWithReturn()
      {
         var weavingModel = new NetAspectWeavingMethod
         {
            OnFinallyInstructions = new List<Instruction> {Instruction.Create(OpCodes.Nop)},
         };
         var assert = new AssertInstructions();
         assert.Add(OpCodes.Nop);
         assert.Add(OpCodes.Ldstr);
         assert.Add(OpCodes.Stloc_0);
         assert.Add(OpCodes.Br_S);
         assert.Add(OpCodes.Ldloc_0);
         assert.Add(OpCodes.Stloc);
         assert.Add(OpCodes.Leave);
         assert.Add(OpCodes.Nop);
         assert.Add(OpCodes.Leave);
         assert.Add(OpCodes.Nop);
         assert.Add(OpCodes.Endfinally);
         assert.Add(OpCodes.Ldloc);
         assert.Add(OpCodes.Ret);

         NetAspectCoreTestHelper.UpdateMethod(GetType(), "FinallyWithReturn", weavingModel, (o, info) => info.Invoke(o, new object[0]), assert);
      }

      [Test]
      public void CheckNopOnFinally()
      {
         var weavingModel = new NetAspectWeavingMethod
         {
            OnFinallyInstructions = new List<Instruction> {Instruction.Create(OpCodes.Nop)}
         };
         var assert = new AssertInstructions();
         assert.Add(OpCodes.Nop);
         assert.Add(OpCodes.Leave);
         assert.Add(OpCodes.Nop);
         assert.Add(OpCodes.Leave);
         assert.Add(OpCodes.Nop);
         assert.Add(OpCodes.Endfinally);
         assert.Add(OpCodes.Ret);
         NetAspectCoreTestHelper.UpdateMethod(GetType(), "NopOnFinally", weavingModel, (o, info) => info.Invoke(o, new object[0]), assert);
      }
   }
}
