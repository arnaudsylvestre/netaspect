using System;
using System.Diagnostics;
using Microsoft.Build.Utilities;
using NetAspect.Weaver.Core.Model.Errors;
using NetAspect.Weaver.Core.Weaver;
using NetAspect.Weaver.Factory;

namespace NetAspect.Weaver.Tasks
{
   public class AppDomainIsolatedDiscoveryRunner : MarshalByRefObject
   {
      public bool Process(string assemblyPath, TaskLoggingHelper logger, out string[] weavedFiles)
      {
         weavedFiles = null;
         try
         {
            WeaverEngine weaverEngine_L = WeaverFactory.Create();
            ErrorReport handler = weaverEngine_L.Weave(assemblyPath, TargetFileName);

            foreach (string warning_L in handler.Warnings)
            {
               logger.LogWarning(warning_L);
            }
            foreach (string error in handler.ErrorsAndFailures)
            {
               logger.LogError(error);
            }


            return handler.Errors.Count == 0;
         }
         catch (Exception e)
         {
             logger.LogError(e.Message);
             logger.LogError(e.StackTrace);
            return false;
         }
      }

      public static string TargetFileName(string sourceFileName)
      {
         return sourceFileName + ".tmp";
      }
   }
}
