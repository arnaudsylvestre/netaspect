using System.Reflection;
using FluentAspect.Weaver.Tests.Core;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Calls.Fields
{
    public class ClassAndAspectAndCallActual
    {
        public NetAspectAttribute Aspect { get; set; }
        private string callerClassName;
        private string callerMethodName;
        private Assembly assembly;

        public ClassAndAspectAndCallActual(Assembly assembly_P, string callerClassName, string callerMethodName)
        {
            assembly = assembly_P;
            this.callerClassName = callerClassName;
            this.callerMethodName = callerMethodName;
        }

        public object CreateCallerObject(params object[] parameters)
        {
            return assembly.CreateObject(callerClassName, parameters);
        }

        public void CallCallerMethod(object o_P, params object[] parameters)
        {
            o_P.CallMethod(callerMethodName, parameters);
        }
    }
}