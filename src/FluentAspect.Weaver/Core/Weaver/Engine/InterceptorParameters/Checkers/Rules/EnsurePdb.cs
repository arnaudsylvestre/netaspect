using System.Reflection;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;
using NetAspect.Weaver.Helpers.Mono.Cecil.IL;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Checkers
{
    public static class EnsurePdb
    {

        public static void IsPresent(Mono.Cecil.Cil.Instruction instruction, ErrorHandler errorHandler, ParameterInfo info)
        {
            if (instruction.GetLastSequencePoint() == null)
                errorHandler.OnError(
                    ErrorCode.NoDebuggingInformationAvailable,
                    FileLocation.None,
                    info.Name,
                    (info.Member).Name,
                    (info.Member.DeclaringType).FullName);
        }
       
    }
}