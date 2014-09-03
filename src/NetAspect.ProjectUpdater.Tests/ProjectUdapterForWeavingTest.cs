using System;
using NUnit.Framework;

namespace NetAspect.ProjectUpdater.Tests
{
   [TestFixture]
   public class ProjectUdapterForWeavingTest
   {
      [Test]
      public void CheckUpdateProject()
      {
         var projectUdapter_L = new ProjectUdapterForWeaving();
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

      public void AddNetAspectSupport()
      {
         throw new NotImplementedException();
      }

      public void Save(string sampleupdatedCsproj_P)
      {
         throw new NotImplementedException();
      }
   }
}
