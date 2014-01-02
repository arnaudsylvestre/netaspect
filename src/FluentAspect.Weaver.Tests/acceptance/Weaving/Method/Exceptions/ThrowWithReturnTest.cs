using System;
using FluentAspect.Sample;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Method.Exceptions
{
   [TestFixture]
   public class ThrowWithReturnTest : AcceptanceTest
   {

      protected override Action Execute()
      {
         return () =>
            {
               try
               {
                  new MyClassToWeave().CheckThrowWithReturn();
               }
               catch (NotSupportedException)
               {
               }
            };
      }
   }
}
