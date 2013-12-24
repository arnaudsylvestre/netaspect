using System;
using FluentAspect.Weaver.Core.Errors;
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
               ErrorHandler handler = new ErrorHandler();
                var weaverCore = WeaverCoreFactory.Create();
                weaverCore.Weave(configFile, handler, TargetFileName);

                foreach (var warning_L in handler.Warnings)
                {
                   logger.LogWarning(warning_L);
                }
                foreach (var error in handler.Errors)
                {
                   logger.LogError(error);
                }


                return handler.Errors.Count == 0;
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