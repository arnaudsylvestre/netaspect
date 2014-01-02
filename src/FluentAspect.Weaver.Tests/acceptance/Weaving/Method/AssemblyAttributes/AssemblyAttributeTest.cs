using System;
using FluentAspect.Sample;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Method.AssemblyAttributes
{
   [TestFixture]
   public class AssemblyAttributeTest : AcceptanceTest
   {

      protected override Action Execute()
      {
         return () =>
            {
               try
               {
                  new MyClassToWeave().WeavedThroughAssembly();
               }
               catch (NotSupportedException e)
               {
                  Assert.AreEqual("Weaved through assembly", e.Message);
               }
            };
      }
   }
}
