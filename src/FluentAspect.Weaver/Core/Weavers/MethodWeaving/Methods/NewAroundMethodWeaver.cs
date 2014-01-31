using System.Collections.Generic;
using System.Linq;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Helpers;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Model;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Methods
{
    public interface IMethodWeaver
    {
        void Init(List<Instruction> initInstructions, Collection<VariableDefinition> variables);
        void InsertBefore(List<Instruction> method);
        void InsertAfter(List<Instruction> afterInstructions);
        void InsertOnException(List<Instruction> onExceptionInstructions);
    }

    public class NewAroundMethodWeaver
    {
        private MethodToWeave method;
        private IMethodWeaver methodWeaver;

        public NewAroundMethodWeaver(MethodToWeave method, IMethodWeaver methodWeaver)
        {
            this.method = method;
            this.methodWeaver = methodWeaver;
        }

        public void Weave()
        {
            var befores = from b in method.Interceptors where b.Before.Method != null select b;
            var afters = from b in method.Interceptors where b.After.Method != null select b;
            var onExceptions = from b in method.Interceptors where b.OnException.Method != null select b;
            if (!befores.Any() && !afters.Any() && !onExceptions.Any())
            {
                return;
            }


            var initInstructions = new List<Instruction>();
            var beforeInstructions = new List<Instruction>();
            var afterInstructions = new List<Instruction>();
            var onExceptionInstructions = new List<Instruction>();
            Variables variables = method.CreateVariables();
            methodWeaver.Init(initInstructions, method.Method.MethodDefinition.Body.Variables);
            methodWeaver.InsertBefore(beforeInstructions);
            methodWeaver.InsertAfter(afterInstructions);
            methodWeaver.InsertOnException(onExceptionInstructions);
            var first = method.Method.MethodDefinition.Body.Instructions.First();
            var last = method.Method.MethodDefinition.Body.Instructions.Last();
            if (onExceptionInstructions.Count() )
                method.Method.AddTryCatch(() => Weave(methodWeaver, method, wrappedMethod, variables),
                                                () => method.GenerateOnExceptionInterceptor(variables));
            else
                Weave(method, wrappedMethod, variables);
            method.Method.Return(variables.handleResult);
        }

        private void Weave(IMethodWeaver methodWeaver, MethodToWeave myMethod, MethodDefinition wrappedMethod, Variables variables)
        {
            myMethod.CallBefore(variables);
            myMethod.CallWeavedMethod(wrappedMethod, variables.handleResult);
            myMethod.CallAfter(variables);
        }
    }
}