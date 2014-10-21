using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace NetAspect.Weaver.Tasks
{
   public class NetAspectWeaverTask : Task
   {
      [Required]
       public string AssemblyPath { get; set; }

      public override bool Execute()
      {
         Stopwatch stopwatch = Stopwatch.StartNew();

         try
         {
             string directoryPath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
             AppDomain domain = AppDomain.CreateDomain(
               "NetAspect Weaving",
               null,
               new AppDomainSetup
               {
                  ApplicationBase = directoryPath,
                  ShadowCopyFiles = "true"
               });

            try
            {
               Type runnerType = typeof (AppDomainIsolatedDiscoveryRunner);

               string assemblyName = new Uri(runnerType.Assembly.CodeBase).LocalPath;
               Log.LogMessage(assemblyName);
                var runner =
                  domain.CreateInstanceFromAndUnwrap(
                     assemblyName,
                     runnerType.FullName) as
                     AppDomainIsolatedDiscoveryRunner;

                string[] weavedFiles;
                if (!runner.Process(AssemblyPath, Log, out weavedFiles))
               {
                   string targetFileName = AppDomainIsolatedDiscoveryRunner.TargetFileName(AssemblyPath);
                  if (File.Exists(targetFileName))
                     File.Delete(targetFileName);
                  return false;
               }
            }
            finally
            {
               AppDomain.Unload(domain);
            }
            string tempFileName = AppDomainIsolatedDiscoveryRunner.TargetFileName(AssemblyPath);
            File.Copy(tempFileName, AssemblyPath, true);
            File.Delete(tempFileName);
         }
         catch (Exception e)
         {
             Log.LogError(e.ToString());
             Log.LogError(e.StackTrace);
            return false;
         }

         stopwatch.Stop();
         return true;
      }
   }
}
