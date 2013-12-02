using System;
using FluentAspect.Weaver.Factory;
using Microsoft.Build.Utilities;

namespace SheepAspect.Tasks
{
    public class AppDomainIsolatedDiscoveryRunner: MarshalByRefObject
    {
        public bool Process(string configFile, TaskLoggingHelper logger, out string[] weavedFiles)
        {
            weavedFiles = null;
            try
            {
                var weaverCore = WeaverCoreFactory.Create();
                weaverCore.Weave(configFile, TargetFileName(configFile));

                return true;
            }
            catch(Exception e)
            {
                logger.LogError(e.Message);
                return false;
            }
        }

        public static string TargetFileName(string sourceFileName)
        {
            return sourceFileName + ".tmp";
        }
    }
}