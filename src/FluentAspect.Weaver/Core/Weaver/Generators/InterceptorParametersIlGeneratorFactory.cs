﻿namespace NetAspect.Weaver.Core.Weaver.Generators
{
    public static class InterceptorParametersIlGeneratorFactory
    {
        //public static void CreateIlGeneratorForInstanceParameter<T>(this ParametersIlGenerator<T> ilGeneratoir,
        //                                                            MethodDefinition method)
        //{
        //    //ilGeneratoir.Add("instance", new InstanceInterceptorParametersIlGenerator<T>());
        //}
        //public static void CreateIlGeneratorForCallerParameter<T>(this ParametersIlGenerator<T> ilGeneratoir)
        //{
        //    //ilGeneratoir.Add("caller", new InstanceInterceptorParametersIlGenerator<T>());
        //}
        //public static void CreateIlGeneratorForColumnNumber<T>(this ParametersIlGenerator<T> ilGeneratoir, Instruction instruction)
        //{
        //    //ilGeneratoir.Add("columnnumber", new SequencePointIntInterceptorParametersIlGenerator<T>(instruction, point => point.StartColumn));
        //}
        //public static void CreateIlGeneratorForLineNumber<T>(this ParametersIlGenerator<T> ilGeneratoir, Instruction instruction)
        //{
        //    //ilGeneratoir.Add("linenumber", new SequencePointIntInterceptorParametersIlGenerator<T>(instruction, point => point.StartLine));
        //}
        //public static void CreateIlGeneratorForFilename<T>(this ParametersIlGenerator<T> ilGeneratoir, Instruction instruction)
        //{
        //    //ilGeneratoir.Add("filename", new SequencePointStringInterceptorParametersIlGenerator<T>(instruction, i => Path.GetFileName(i.Document.Url)));
        //}
        //public static void CreateIlGeneratorForFilePath<T>(this ParametersIlGenerator<T> ilGeneratoir, Instruction instruction)
        //{
        //    //ilGeneratoir.Add("filepath", new SequencePointStringInterceptorParametersIlGenerator<T>(instruction, i => i.Document.Url));
        //}
        //public static void CreateIlGeneratorForField(this ParametersIlGenerator<IlInjectorAvailableVariablesForInstruction> ilGeneratoir, Instruction instruction, ModuleDefinition module) 
        //{
        //    //ilGeneratoir.Add("field", new FieldInterceptorParametersIlGenerator(instruction, module));
        //}
        //public static void CreateIlGeneratorForProperty(this ParametersIlGenerator<IlInjectorAvailableVariablesForInstruction> ilGeneratoir, PropertyDefinition property, ModuleDefinition module) 
        //{
        //    //ilGeneratoir.Add("property", new PropertyPInterceptorParametersIlGenerator(property, module));
        //}
        //public static void CreateIlGeneratorForCallerParameters(this ParametersIlGenerator<IlInjectorAvailableVariablesForInstruction> ilGeneratoir)
        //{
        //    //ilGeneratoir.Add("callerparameters", new ParametersInterceptorParametersIlGenerator<IlInjectorAvailableVariablesForInstruction>());
        //}
        
        //public static void CreateIlGeneratorForCallerParametersName(this ParametersIlGenerator<IlInjectorAvailableVariablesForInstruction> ilGeneratoir,
        //                                                            MethodDefinition callerMethod)
        //{
        //    //foreach (ParameterDefinition parameterDefinition in callerMethod.Parameters)
        //    //{
        //    //    ilGeneratoir.Add("caller" + parameterDefinition.Name.ToLower(),
        //    //                     new ParameterNameInterceptorParametersIlGenerator<IlInjectorAvailableVariablesForInstruction>(parameterDefinition));
        //    //}
        //}
        //public static void 
        //    CreateIlGeneratorForCalledParameter(this ParametersIlGenerator<IlInjectorAvailableVariablesForInstruction> ilGeneratoir)
        //{
        //    //ilGeneratoir.Add("called", new CalledInterceptorParametersIlGenerator());
        //}

        //public static void CreateIlGeneratorForMethodParameter<T>(
        //    this ParametersIlGenerator<T> ilGeneratoir) where T : IlInstructionInjectorAvailableVariables
        //{
        //    //ilGeneratoir.Add("method", new MethodInterceptorParametersIlGenerator<T>());
        //}
        //public static void CreateIlGeneratorForConstructorParameter<T>(
        //    this ParametersIlGenerator<T> ilGeneratoir) where T : IlInstructionInjectorAvailableVariables
        //{
        //    //ilGeneratoir.Add("constructor", new MethodInterceptorParametersIlGenerator<T>());
        //}

        //public static void CreateIlGeneratorForPropertyParameter(
        //    this ParametersIlGenerator<IlInjectorAvailableVariables> ilGeneratoir)
        //{
        //    //ilGeneratoir.Add("property", new PropertyInterceptorParametersIlGenerator());
        //}

        //public static void CreateIlGeneratorForResultParameter(
        //    this ParametersIlGenerator<IlInjectorAvailableVariables> ilGeneratoir)
        //{
        //    //ilGeneratoir.Add("result", new ResultInterceptorParametersIlGenerator());
        //}

        //public static void CreateIlGeneratorForExceptionParameter(
        //    this ParametersIlGenerator<IlInjectorAvailableVariables> ilGeneratoir)
        //{
        //    //ilGeneratoir.Add("exception", new ExceptionInterceptorParametersIlGenerator());
        //}

        //public static void CreateIlGeneratorForParameterNameParameter<T>(this ParametersIlGenerator<T> ilGeneratoir,
        //                                                                 MethodDefinition method)
        //{
        //    //foreach (ParameterDefinition parameterDefinition in method.Parameters)
        //    //{
        //    //    ilGeneratoir.Add(parameterDefinition.Name.ToLower(),
        //    //                     new ParameterNameInterceptorParametersIlGenerator<T>(parameterDefinition));
        //    //}
        //}


        //public static void CreateIlGeneratorForPropertySetValueParameter<T>(this ParametersIlGenerator<T> ilGeneratoir,
        //                                                                    MethodDefinition method)
        //{
        //    //ilGeneratoir.Add("value", new ParameterNameInterceptorParametersIlGenerator<T>(method.Parameters[0]));
        //}

        //public static void CreateIlGeneratorForParametersParameter(
        //    this ParametersIlGenerator<IlInjectorAvailableVariables> ilGeneratoir, MethodDefinition method)
        //{
        //    //ilGeneratoir.Add("parameters", new ParametersInterceptorParametersIlGenerator<IlInjectorAvailableVariables>());
        //}
    }
}