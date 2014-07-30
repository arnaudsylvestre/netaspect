namespace NetAspect.Weaver.Tests.docs.MethodPossibilities.Before
{
   [ParameterDescription("instance", "The instance", "MethodWeaving")]
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


   [PossibilityDocumentation("MethodWeaving", "PossibilityDescription", "On methods")]
   [PossibilityDocumentation("ConstructorWeaving", "PossibilityDescription", "On constructors")]
   [PossibilityDocumentation("PropertyGetWeaving", "PossibilityDescription", "On properties (For getter)")]
   [PossibilityDocumentation("PropertySetWeaving", "PossibilityDescription", "On properties (For setter)")]











   [PossibilityDocumentation("InstructionMethodWeaving", "PossibilityDescription", "On methods")]
   [PossibilityDocumentation("InstructionConstructorWeaving", "PossibilityDescription", "On constructors")]
   [PossibilityDocumentation("InstructionPropertyGetWeaving", "PossibilityDescription", "On properties (For getter)")]
   [PossibilityDocumentation("InstructionPropertySetWeaving", "PossibilityDescription", "On properties (For setter)")]

   public class DocConfiguration
   {
      
   }
}