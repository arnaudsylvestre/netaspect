using System;
using System.Reflection;
using FluentAspect.Sample;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests
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
