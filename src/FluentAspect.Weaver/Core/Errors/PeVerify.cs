
using FluentAspect.Weaver.Core.Helpers;

namespace FluentAspect.Weaver.Core.Errors
{
    public class PEVerify
    {
        public static void Run(string assemblyFile)
        {
            ProcessHelper.Launch("peverify.exe", "\"" + assemblyFile + "\"");
            
        }

        
    }
}