using System;
using System.Diagnostics;

namespace FluentAspect.Weaver.Core.Helpers
{
   public static class ProcessHelper
   {
      public static void Launch(string filename, string arguments)
      {
         var p = new Process
         {
            StartInfo =
            {
               Arguments = arguments,
               CreateNoWindow = true,
               UseShellExecute = false,
               RedirectStandardOutput = true,
               RedirectStandardInput = true,
               RedirectStandardError = true,
               FileName = filename
            } 
         };
         p.Start();
         p.WaitForExit();
         var output = p.StandardOutput.ReadToEnd();
         var exitCode = p.ExitCode;
         if (0 != exitCode)
         {
            throw new Exception(output);
         }
      }
   }
}