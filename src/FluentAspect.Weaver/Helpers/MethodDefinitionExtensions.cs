using Mono.Cecil;

namespace FluentAspect.Weaver.Helpers
{
    public static class MethodDefinitionExtensions
    {
        public static MethodDefinition Clone(this MethodDefinition methodDefinition, string cloneMethodName)
        {
            var wrappedMethod = new MethodDefinition(cloneMethodName, methodDefinition.Attributes, methodDefinition.ReturnType);

            foreach (var parameterDefinition in methodDefinition.Parameters)
            {
                wrappedMethod.Parameters.Add(parameterDefinition);
            }
            foreach (var instruction in methodDefinition.Body.Instructions)
            {
                wrappedMethod.Body.Instructions.Add(instruction);
            }
            methodDefinition.Body.Instructions.Clear();
            foreach (var variable in methodDefinition.Body.Variables)
            {
                wrappedMethod.Body.Variables.Add(variable);
            }
            wrappedMethod.Body.InitLocals = methodDefinition.Body.InitLocals;
            methodDefinition.Body.Variables.Clear();
            foreach (var exceptionHandler in methodDefinition.Body.ExceptionHandlers)
            {
                wrappedMethod.Body.ExceptionHandlers.Add(exceptionHandler);
            }
            foreach (var genericParameter in methodDefinition.GenericParameters)
            {
                wrappedMethod.GenericParameters.Add(new GenericParameter(genericParameter.Name, wrappedMethod));
            }
            return wrappedMethod;
        }
         
    }
}