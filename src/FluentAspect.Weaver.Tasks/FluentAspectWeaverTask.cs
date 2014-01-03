using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace FluentAspect.Weaver.Tasks
{
    public class FluentAspectWeaverTask : Task
    {
        [Required]
        public string ConfigFile { get; set; }

        public override bool Execute()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            try
            {
                string directoryPath = Path.GetDirectoryName(ConfigFile);
                string[] weavedFiles;
                AppDomain domain = AppDomain.CreateDomain("SheepAspect Weaving", null,
                                                          new AppDomainSetup
                                                              {
                                                                  ApplicationBase = directoryPath,
                                                                  ShadowCopyFiles = "true"
                                                              });

                try
                {
                    Type runnerType = typeof (AppDomainIsolatedDiscoveryRunner);

                    var runner =
                        domain.CreateInstanceFromAndUnwrap(new Uri(runnerType.Assembly.CodeBase).LocalPath,
                                                           runnerType.FullName) as
                        AppDomainIsolatedDiscoveryRunner;

                    if (!runner.Process(ConfigFile, Log, out weavedFiles))
                    {
                        string targetFileName = AppDomainIsolatedDiscoveryRunner.TargetFileName(ConfigFile);
                        if (File.Exists(targetFileName))
                            File.Delete(targetFileName);
                        return false;
                    }
                }
                finally
                {
                    AppDomain.Unload(domain);
                }
                string tempFileName = AppDomainIsolatedDiscoveryRunner.TargetFileName(ConfigFile);
                File.Copy(tempFileName, ConfigFile, true);
                File.Delete(tempFileName);
            }
            catch (Exception e)
            {
                Log.LogError(e.ToString());
                return false;
            }

            stopwatch.Stop();
            Log.LogMessage("[SheepAspect-Compiler] Compiled {0} in {1:n} ms", ConfigFile, stopwatch.ElapsedMilliseconds);
            return true;
        }
    }
}