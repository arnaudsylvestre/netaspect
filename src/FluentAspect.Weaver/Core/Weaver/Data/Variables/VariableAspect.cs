using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Weaver.Engine.LifeCycle;

namespace NetAspect.Weaver.Core.Weaver.Data.Variables
{
    public class VariableAspect : Variable.IVariableBuilder
    {

        private readonly AspectBuilder aspectBuilder;
        private NetAspectDefinition aspect;

        public VariableAspect(AspectBuilder aspectBuilder)
        {
            this.aspectBuilder = aspectBuilder;
        }

        public VariableDefinition Build(InstructionsToInsert instructionsToInsert_P, MethodDefinition method, Instruction instruction)
        {
            var interceptorVariable = new VariableDefinition(method.Module.Import(aspect.Type));
            aspectBuilder.CreateInterceptor(aspect, method, interceptorVariable, instructionsToInsert_P.aspectInitialisation);
            return interceptorVariable;
        }
    }
}