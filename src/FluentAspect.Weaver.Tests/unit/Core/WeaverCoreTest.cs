using System.Reflection;
using FluentAspect.Weaver.Core;
using Mono.Cecil;
using Moq;
using NUnit.Framework;

namespace FluentAspect.Weaver.Tests.unit.Core
{
   [TestFixture]
   public class WeaverCoreTest
   {
      [Test]
      public void Check()
      {
         var currentAssembly = Assembly.GetExecutingAssembly();
         
         var configurationReader = new Mock<IConfigurationReader>();
         configurationReader.Setup(reader => reader.ReadConfiguration(currentAssembly.GetTypes()));

         var weaverBuilder = new Mock<IWeaverBuilder>();
         weaverBuilder.Setup(builder => builder.BuildWeavers(It.IsAny<AssemblyDefinition>(), It.IsAny<WeavingConfiguration>()));


         WeaverCore weaver = new WeaverCore(configurationReader.Object, weaverBuilder.Object);
         weaver.Weave();
      }
   }
}
