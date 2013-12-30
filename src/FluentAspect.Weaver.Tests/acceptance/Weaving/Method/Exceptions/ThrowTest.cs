using System;
using System.Reflection;
using FluentAspect.Sample;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests
{
   [TestFixture]
   public class ThrowTest : AcceptanceTest
   {

      protected override Action Execute()
      {
         return () =>
            {
               try
               {
                  new MyClassToWeave().CheckThrow();
               }
               catch (NotSupportedException)
               {
               }
            };
      }
   }
}
