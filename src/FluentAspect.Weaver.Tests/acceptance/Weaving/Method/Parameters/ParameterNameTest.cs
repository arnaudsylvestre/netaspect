using System;
using FluentAspect.Sample;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests
{
   [TestFixture]
   public class ParameterNameTest : AcceptanceTest
   {

      protected override Action Execute()
      {
         return () =>
            {
               string res = new MyClassToWeave().CheckWithParameterName(6, 1);
               Assert.AreEqual("6 : 7", res);
            };
      }
   }
}
