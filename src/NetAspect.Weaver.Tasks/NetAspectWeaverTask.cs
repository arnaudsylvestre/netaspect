using System;
using System.Diagnostics;
using System.IO;
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
             string directoryPath = Path.GetDirectoryName(AssemblyPath);
            string[] weavedFiles;
            AppDomain domain = AppDomain.CreateDomain(
               "SheepAspect Weaving",
               null,
               new AppDomainSetup
               {
                  ApplicationBase = directoryPath,
                  ShadowCopyFiles = "true"
               });

            try
            {
               Type runnerType = typeof (AppDomainIsolatedDiscoveryRunner);

               var runner =
                  domain.CreateInstanceFromAndUnwrap(
                     new Uri(runnerType.Assembly.CodeBase).LocalPath,
                     runnerType.FullName) as
                     AppDomainIsolatedDiscoveryRunner;

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
            return false;
         }

         stopwatch.Stop();
         Log.LogMessage("[SheepAspect-Compiler] Compiled {0} in {1:n} ms", AssemblyPath, stopwatch.ElapsedMilliseconds);
         return true;
      }
   }
}
