using System;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Weaver.Engine;
using FluentAspect.Weaver.Factory;
using Microsoft.Build.Utilities;

namespace FluentAspect.Weaver.Tasks
{
    public class AppDomainIsolatedDiscoveryRunner : MarshalByRefObject
    {
        public bool Process(string configFile, TaskLoggingHelper logger, out string[] weavedFiles)
        {
            weavedFiles = null;
            try
            {
                var handler = new ErrorHandler();
                WeaverCore weaverCore = WeaverCoreFactory.Create();
                weaverCore.Weave(configFile, handler, TargetFileName);

                foreach (string warning_L in handler.Warnings)
                {
                    logger.LogWarning(warning_L);
                }
                foreach (string error in handler.Errors)
                {
                    logger.LogError(error);
                }


                return handler.Errors.Count == 0;
            }
            catch (Exception e)
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