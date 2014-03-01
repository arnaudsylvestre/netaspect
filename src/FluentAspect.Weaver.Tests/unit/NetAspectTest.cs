using System;
using FluentAspect.Weaver.Core.Errors;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit
{
   [TestFixture]
   public abstract class NetAspectTest<T>
   {
      [Test]
      public void DoTest()
      {
         DoUnit.Run<T>(CreateErrorHandlerProvider(), CreateEnsure());
      }

      protected virtual Action CreateEnsure()
      {
         return () =>
            {
            };
      }

      protected virtual Action<ErrorHandler> CreateErrorHandlerProvider()
      {
         return e => { };
      }
   }

   
    
}
