using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace NetAspect.ProjectUpdater.Tests
{
   [TestFixture]
   public class ProjectUdapterForWeavingTest
   {
      [Test]
      public void CheckUpdateProject()
      {
         ProjectUdapterForWeaving projectUdapter_L = new ProjectUdapterForWeaving();
         projectUdapter_L.Load("Sample.csproj");
         projectUdapter_L.AddNetAspectSupport();
         projectUdapter_L.Save("SampleUpdated.csproj");
         FileAssert.AreEqual("Expected.csproj", "SampleUpdated.csproj");
      }
   }

   public class ProjectUdapterForWeaving
   {
      public void Load(string pathToFile)
      {
         
      }
   }
}
