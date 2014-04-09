using System;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Call;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.Generators
{
    public static class InterceptorParametersIlGeneratorFactory
    {
        public static void CreateIlGeneratorForInstanceParameter<T>(this ParametersIlGenerator<T> ilGeneratoir,
                                                                    MethodDefinition method)
        {
            ilGeneratoir.Add("instance", new InstanceInterceptorParametersIlGenerator<T>());
        }
        public static void CreateIlGeneratorForCallerParameter<T>(this ParametersIlGenerator<T> ilGeneratoir)
        {
            ilGeneratoir.Add("caller", new InstanceInterceptorParametersIlGenerator<T>());
        }
        public static void CreateIlGeneratorForColumnNumber<T>(this ParametersIlGenerator<T> ilGeneratoir, Instruction instruction)
        {
            ilGeneratoir.Add("columnnumber", new SequencePointIntInterceptorParametersIlGenerator<T>(instruction, point => point.StartColumn));
        }
        public static void CreateIlGeneratorForLineNumber<T>(this ParametersIlGenerator<T> ilGeneratoir, Instruction instruction)
        {
            ilGeneratoir.Add("linenumber", new SequencePointIntInterceptorParametersIlGenerator<T>(instruction, point => point.StartLine));
        }
        public static void CreateIlGeneratorForFilename<T>(this ParametersIlGenerator<T> ilGeneratoir, Instruction instruction)
        {
            ilGeneratoir.Add("filename", new SequencePointStringInterceptorParametersIlGenerator<T>(instruction, i => Path.GetFileName(i.Document.Url)));
        }
        public static void CreateIlGeneratorForFilePath<T>(this ParametersIlGenerator<T> ilGeneratoir, Instruction instruction)
        {
            ilGeneratoir.Add("filepath", new SequencePointStringInterceptorParametersIlGenerator<T>(instruction, i => i.Document.Url));
        }
        public static void CreateIlGeneratorForField<T>(this ParametersIlGenerator<T> ilGeneratoir, Instruction instruction, ModuleDefinition module) where T : IlInstructionInjectorAvailableVariables
        {
            ilGeneratoir.Add("field", new FieldInterceptorParametersIlGenerator<T>(instruction, module));
        }
        public static void CreateIlGeneratorForCallerParameters(this ParametersIlGenerator<IlInjectorAvailableVariablesForInstruction> ilGeneratoir)
        {
            ilGeneratoir.Add("callerparameters", new ParametersInterceptorParametersIlGenerator<IlInjectorAvailableVariablesForInstruction>());
        }
        public static void CreateIlGeneratorForCalledParameters(this ParametersIlGenerator<IlInjectorAvailableVariablesForInstruction> ilGeneratoir)
        {
            ilGeneratoir.Add("calledparameters", new CalledParametersInterceptorParametersIlGenerator());
        }
        public static void CreateIlGeneratorForCalledParametersName(this ParametersIlGenerator<IlInjectorAvailableVariablesForInstruction> ilGeneratoir,
                                                                    MethodDefinition calledMethod)
        {
            foreach (ParameterDefinition parameterDefinition in calledMethod.Parameters)
            {
                ilGeneratoir.Add("called" + parameterDefinition.Name.ToLower(),
                                 new CalledParameterNameInterceptorParametersIlGenerator("called" + parameterDefinition.Name.ToLower()));
            }
        }
        public static void CreateIlGeneratorForCallerParametersName(this ParametersIlGenerator<IlInjectorAvailableVariablesForInstruction> ilGeneratoir,
                                                                    MethodDefinition callerMethod)
        {
            foreach (ParameterDefinition parameterDefinition in callerMethod.Parameters)
            {
                ilGeneratoir.Add("caller" + parameterDefinition.Name.ToLower(),
                                 new ParameterNameInterceptorParametersIlGenerator<IlInjectorAvailableVariablesForInstruction>(parameterDefinition));
            }
        }
        public static void CreateIlGeneratorForCalledParameter(this ParametersIlGenerator<IlInjectorAvailableVariablesForInstruction> ilGeneratoir)
        {
            ilGeneratoir.Add("called", new CalledInterceptorParametersIlGenerator());
        }

        public static void CreateIlGeneratorForMethodParameter(
            this ParametersIlGenerator<IlInjectorAvailableVariables> ilGeneratoir)
        {
            ilGeneratoir.Add("method", new MethodInterceptorParametersIlGenerator());
        }

        public static void CreateIlGeneratorForPropertyParameter(
            this ParametersIlGenerator<IlInjectorAvailableVariables> ilGeneratoir)
        {
            ilGeneratoir.Add("property", new PropertyInterceptorParametersIlGenerator());
        }

        public static void CreateIlGeneratorForResultParameter(
            this ParametersIlGenerator<IlInjectorAvailableVariables> ilGeneratoir)
        {
            ilGeneratoir.Add("result", new ResultInterceptorParametersIlGenerator());
        }

        public static void CreateIlGeneratorForExceptionParameter(
            this ParametersIlGenerator<IlInjectorAvailableVariables> ilGeneratoir)
        {
            ilGeneratoir.Add("exception", new ExceptionInterceptorParametersIlGenerator());
        }

        public static void CreateIlGeneratorForParameterNameParameter<T>(this ParametersIlGenerator<T> ilGeneratoir,
                                                                         MethodDefinition method)
        {
            foreach (ParameterDefinition parameterDefinition in method.Parameters)
            {
                ilGeneratoir.Add(parameterDefinition.Name.ToLower(),
                                 new ParameterNameInterceptorParametersIlGenerator<T>(parameterDefinition));
            }
        }


        public static void CreateIlGeneratorForPropertySetValueParameter<T>(this ParametersIlGenerator<T> ilGeneratoir,
                                                                            MethodDefinition method)
        {
            ilGeneratoir.Add("value", new ParameterNameInterceptorParametersIlGenerator<T>(method.Parameters[0]));
        }

        public static void CreateIlGeneratorForParametersParameter(
            this ParametersIlGenerator<IlInjectorAvailableVariables> ilGeneratoir, MethodDefinition method)
        {
            ilGeneratoir.Add("parameters", new ParametersInterceptorParametersIlGenerator<IlInjectorAvailableVariables>());
        }
    }
}