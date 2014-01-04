using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Helpers;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Model;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine
{
    public class WeavedMethodBuilder
    {
        public void Build(MethodToWeave weavedMethod, MethodDefinition wrappedMethod)
        {
            Variables variables = weavedMethod.CreateVariables();
            if (weavedMethod.HasInterceptorsOnException())
                weavedMethod.Method.AddTryCatch(() => Weave(weavedMethod, wrappedMethod, variables),
                                                () => weavedMethod.GenerateOnExceptionInterceptor(variables));
            else
                Weave(weavedMethod, wrappedMethod, variables);
            weavedMethod.Method.Return(variables.handleResult);
        }

        private void Weave(MethodToWeave myMethod, MethodDefinition wrappedMethod, Variables variables)
        {
            myMethod.CallBefore(variables);
            myMethod.CallWeavedMethod(wrappedMethod, variables.handleResult);
            myMethod.CallAfter(variables);
        }
    }
}