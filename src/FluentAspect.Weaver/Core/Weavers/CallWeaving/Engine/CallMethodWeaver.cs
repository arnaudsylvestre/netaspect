using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Model.Helpers;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine.Model;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine
{
    public class CallMethodWeaver : IWeaveable
    {
        private MethodCallToWeave toWeave;
        private ParametersEngine engine;

        public CallMethodWeaver(ParametersEngine engine, MethodCallToWeave toWeave)
        {
            this.engine = engine;
            this.toWeave = toWeave;
        }


        public void Weave()
        {
            var reference = toWeave.CalledMethod;
            toWeave.JoinPoint.Method.Body.InitLocals = true;
            

        }

        public void Check(ErrorHandler errorHandler)
        {
            foreach (var netAspectAttribute in toWeave.Interceptors)
            {
                engine.Check(netAspectAttribute.BeforeInterceptor.GetParameters(), errorHandler);
                engine.Check(netAspectAttribute.AfterInterceptor.GetParameters(), errorHandler);
            }
        }

        public class KeyValue
        {
            public VariableDefinition Variable;
            public string ParameterName;
        }

        public bool CanWeave()
        {
            return true;
        }

        
    }
}