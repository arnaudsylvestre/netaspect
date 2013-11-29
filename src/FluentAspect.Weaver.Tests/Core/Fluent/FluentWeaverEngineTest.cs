using System;
using FluentAspect.Core;
using FluentAspect.Weaver.Core;
using FluentAspect.Weaver.Core.Fluent;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.Core.Fluent
{
   [TestFixture]
   public class FluentWeaverEngineTest
   {
      [Test, ExpectedException(typeof(ConfigurationNotFoundException))]
      public void CheckWithNoConfiguration()
      {
         FluentConfigurationReader engine = new FluentConfigurationReader();
         engine.ReadConfiguration(new Type[0]);
      }

      [Test]
      public void CheckConfiguration()
      {
         FluentConfigurationReader engine = new FluentConfigurationReader();
         var configuration = engine.ReadConfiguration(new[] { typeof(MyConfiguration) });

         Assert.AreEqual(1, configuration.Methods.Count);
      }
       
      private class MyClassToWeave
      {

         public void Hello()
         {
            
         }
         
      }


      private class MyConfiguration : FluentAspectDefinition
      {
         public override void Setup()
         {
            WeaveMethodWhichMatches<MyInterceptor>(method_P => method_P.Name == "Hello" && method_P.DeclaringType.Name == typeof(MyClassToWeave).Name);
         }
      }

      private class MyInterceptor
      {
          
      }
   }
}