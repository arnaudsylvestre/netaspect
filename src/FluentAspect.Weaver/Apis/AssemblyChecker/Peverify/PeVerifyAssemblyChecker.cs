using System;
using NetAspect.Core.Helpers;
using NetAspect.Weaver.Core.Errors;

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
                errorHandler.Errors.Add("An internal error has occured : " + e.Message);
            }

        }
    }
}