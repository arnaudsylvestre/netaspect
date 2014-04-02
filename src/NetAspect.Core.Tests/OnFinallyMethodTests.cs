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
        public void CheckNopOnfinally()
        {
            var weavingModel = new NetAspectWeavingMethod()
            {
                OnFinallyInstructions = new List<Instruction> { Instruction.Create(OpCodes.Nop) }
            };
            NetAspectCoreTestHelper.UpdateMethod(GetType(), "EmptyMethod", weavingModel, (o, info) => info.Invoke(o, new object[0]));
        }

        

        public void EmptyMethod()
        {
            
        }
    }
}
