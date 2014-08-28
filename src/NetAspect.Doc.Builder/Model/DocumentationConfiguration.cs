using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace NetAspect.Doc.Builder.Model
{
    /*[ParameterDescription("instance", "The instance", "MethodWeaving")]
    [ParameterDescription("method", "The method", "MethodWeaving")]
    [ParameterDescription("property", "The property", "MethodWeaving")]
    [ParameterDescription("constructor", "The constructor", "MethodWeaving")]
    [ParameterDescription("parameters", "The parameters", "MethodWeaving")]
    [ParameterDescription("parameter name", "The parameter's name", "MethodWeaving")]
    [ParameterDescription("result", "The result", "MethodWeaving")]
    [ParameterDescription("exception", "The exception raised", "MethodWeaving")]
    [ParameterDescription("value", "The property value", "MethodWeaving")]


    [InterceptorDescription("Before", "before the method is executed")]
    [InterceptorDescription("After", "after the method is executed")]
    [InterceptorDescription("OnException", "when an exception occured in the method")]
    [InterceptorDescription("OnFinally", "after the method is executed when an exception occured or not")]

    [InterceptorDescription("BeforeConstructor", "before the constructor is executed")]
    [InterceptorDescription("AfterConstructor", "after the constructor is executed")]
    [InterceptorDescription("OnExceptionConstructor", "when an exception occured in the constructor")]
    [InterceptorDescription("OnFinallyConstructor", "after the constructor is executed when an exception occured or not")]

    [InterceptorDescription("BeforePropertyGetMethod", "before the get method of the property is executed")]
    [InterceptorDescription("AfterPropertyGetMethod", "after the get method of the property is executed")]
    [InterceptorDescription("OnExceptionPropertyGetMethod", "when an exception occured in the get method of the property")]
    [InterceptorDescription("OnFinallyPropertyGetMethod", "after the get method of the property is executed when an exception occured or not")]

    [InterceptorDescription("BeforePropertySetMethod", "before the set method of the property is executed")]
    [InterceptorDescription("AfterPropertySetMethod", "after the set method of the property is executed")]
    [InterceptorDescription("OnExceptionPropertySetMethod", "when an exception occured in the set method of the property")]
    [InterceptorDescription("OnFinallyPropertySetMethod", "after the set method of the property is executed when an exception occured or not")]


    [PossibilityDocumentation("MethodWeaving", "PossibilityDescription", "On methods", "MethodWeaving")]
    [PossibilityDocumentation("ConstructorWeaving", "PossibilityDescription", "On constructors", "MethodWeaving")]
    [PossibilityDocumentation("PropertyGetWeaving", "PossibilityDescription", "On properties (For getter)", "MethodWeaving")]
    [PossibilityDocumentation("PropertySetWeaving", "PossibilityDescription", "On properties (For setter)", "MethodWeaving")]











    [PossibilityDocumentation("InstructionFieldWeaving", "PossibilityDescription", "On fields", "InstructionWeaving")]
    [PossibilityDocumentation("InstructionMethodWeaving", "PossibilityDescription", "On methods", "InstructionWeaving")]
    [PossibilityDocumentation("InstructionPropertyWeaving", "PossibilityDescription", "On properties", "InstructionWeaving")]


    [InterceptorDescription("BeforeGetField", "before an instruction get the value of a field")]
    [InterceptorDescription("AfterGetField", "after an instruction get the value of a field")]
    [InterceptorDescription("BeforeUpdateField", "before an instruction set the value of a field")]
    [InterceptorDescription("AfterUpdateField", "after an instruction set the value of a field")]

    [InterceptorDescription("BeforeCallMethod", "before an instruction call a method")]
    [InterceptorDescription("AfterCallMethod", "after an instruction call a method")]

    [InterceptorDescription("BeforeGetProperty", "before an instruction get the value of a property")]
    [InterceptorDescription("AfterGetProperty", "after an instruction get the value of a property")]
    [InterceptorDescription("BeforeSetProperty", "before an instruction set the value of a property")]
    [InterceptorDescription("AfterSetProperty", "after an instruction set the value of a property")]



    [ParameterDescription("caller", "the <i>this</i> of the method which call our member", "InstructionWeaving")]
    [ParameterDescription("called", "the <i>this</i> of our member", "InstructionWeaving")]
    [ParameterDescription("field", "The weaved field", "InstructionWeaving")]
    [ParameterDescription("columnNumber", "The column number in the source file of the instruction", "InstructionWeaving")]
    [ParameterDescription("lineNumber", "The line number in the source file of the instruction", "InstructionWeaving")]
    [ParameterDescription("fileName", "The file name of the source file of the instruction", "InstructionWeaving")]
    [ParameterDescription("filePath", "The file path of the source file of the instruction", "InstructionWeaving")]
    [ParameterDescription("callerParameters", "The parameters of the method which contains the weaved instruction", "InstructionWeaving")]
    [ParameterDescription("callerMethod", "The method which contains the weaved instruction", "InstructionWeaving")]
    [ParameterDescription("caller + parameter name", "the value of the parameter of the method which contains the weaved instruction", "InstructionWeaving")]


    [PossibilityDocumentation("ParameterWeaving", "PossibilityDescription", "On Parameters", "ParameterWeaving")]

    [InterceptorDescription("BeforeMethodForParameter", "before the constructor is executed")]
    [InterceptorDescription("AfterMethodForParameter", "after the constructor is executed")]
    [InterceptorDescription("OnExceptionMethodForParameter", "when an exception occured in the constructor")]
    [InterceptorDescription("OnFinallyMethodForParameter", "after the constructor is executed when an exception occured or not")]*/

    public class InterceptorModel
    {
        public InterceptorModel()
        {
            Parameters = new List<Parameter>();
        }

        public string Name { get; set; }
        public class Parameter
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }
        public List<Parameter> Parameters { get; set; }
        public Event Event { get { return InterceptorModelHelper.ExtractEvent(Name); } }
        public Kind Kind { get { return InterceptorModelHelper.ExtractKind(Name); } }
    }

    public enum Event
    {
        Before,
        After,
        OnException,
        OnFinally,
    }

    public enum Kind
    {
        Method,
        Call,
        Parameter,
    }

    public static class InterceptorModelHelper
    {
        public static Event ExtractEvent(string interceptorName)
        {
            if (interceptorName.Contains("Before"))
                return Event.Before;
            if (interceptorName.Contains("After"))
                return Event.After;
            if (interceptorName.Contains("Exception"))
                return Event.OnException;
            if (interceptorName.Contains("Finally"))
                return Event.OnFinally;
            throw new NotSupportedException(interceptorName);
        }

        public static Kind ExtractKind(string interceptorName)
        {
            if (interceptorName.Contains("Call"))
                return Kind.Call;
            if (interceptorName.Contains("Parameter"))
                return Kind.Parameter;
            return Kind.Method;
        }
    }

    public class DocumentationConfiguration
    {
        public List<InterceptorKind> InterceptorKinds { get; set; }
    }

    public class InterceptorKindConfiguration
    {
        public List<ParameterConfiguration> Parameters { get; set; }
        public List<InterceptorConfiguration> Interceptors { get; set; }

        public string Title { get; set; }
    }

    public class InterceptorKind
    {
        public string Name { get; set; }
        public List<InterceptorKindConfiguration> Configurations { get; set; }
    }

    public class InterceptorConfiguration
    {
        public string MethodName { get; set; }
        public string When { get; set; }
    }

    public class ParameterConfiguration
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}