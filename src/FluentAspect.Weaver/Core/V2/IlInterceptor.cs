using System.Reflection;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.V2
{
    public class IlInterceptor
    {
        public VariableDefinition Variable;
        public MethodInfo MethodToCall;
    }
}