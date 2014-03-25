﻿using System;
using FluentAspect.Weaver.Core.Errors;

namespace FluentAspect.Weaver.Apis.AssemblyChecker.Peverify
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
                errorHandler.Errors.Add("An internal error has occured : " + e.Message);
            }

        }
    }
}