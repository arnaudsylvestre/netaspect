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
            NetAspectCoreTestHelper.UpdateMethod(GetType(), "NopOnFinally", weavingModel, (o, info) => info.Invoke(o, new object[0]));
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
            NetAspectCoreTestHelper.UpdateMethod(GetType(), "ExceptionAndFinally", weavingModel, (o, info) => info.Invoke(o, new object[0]));
        }



        public void ExceptionAndFinally()
        {
            
        }


        public void ExceptionAndFinallyReal()
        {
            try
            {

            }
            catch (Exception)
            {

            }
            finally
            {
                
            }
        }
    }
}
