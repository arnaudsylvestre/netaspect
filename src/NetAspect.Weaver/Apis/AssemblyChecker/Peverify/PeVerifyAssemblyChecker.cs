using System;
using System.IO;
using System.Reflection;
using NetAspect.Core.Helpers;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;
using NetAspect.Weaver.Helpers.NetFramework;

namespace NetAspect.Weaver.Apis.AssemblyChecker.Peverify
{
   public class PeVerifyAssemblyChecker : IAssemblyChecker
   {
      public void Check(string assemblyFile, ErrorHandler errorHandler)
      {
         try
         {
             var assemblyPath = Assembly.GetExecutingAssembly().GetAssemblyPath();
             var peverifyPath = Path.Combine(Path.GetDirectoryName(assemblyPath), "peverify.exe");
             ProcessHelper.Launch(peverifyPath, "\"" + assemblyFile + "\"");
         }
         catch (Exception e)
         {
             var runningOnMono = Type.GetType ("Mono.Runtime") != null;
             if (!runningOnMono)
                errorHandler.OnError(ErrorCode.AssemblyGeneratedIsNotCompliant, FileLocation.None, e.Message);
         }
      }
   }
}
