﻿using System;
using NetAspect.Core.Helpers;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Apis.AssemblyChecker.Peverify
{
   public class PeVerifyAssemblyChecker : IAssemblyChecker
   {
      public void Check(string assemblyFile, ErrorHandler errorHandler)
      {
         try
         {
            ProcessHelper.Launch("peverify.exe", "\"" + assemblyFile + "\"");
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
