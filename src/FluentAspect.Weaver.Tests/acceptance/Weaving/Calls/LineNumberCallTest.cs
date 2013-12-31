﻿using System;
using FluentAspect.Sample;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests
{
   [TestFixture]
   public class LineNumberCallTest : AcceptanceTest
   {

      protected override Action Execute()
      {
         return () =>
            {
               try
               {
                  new MyClassToWeave().CallWeavedOnCallAfter();
                  Assert.Fail();
               }
               catch (Exception e)
               {
                  Assert.AreEqual("83 : 12 : MyClassToWeave.cs", e.Message);
               }
            };
      }
   }
}
