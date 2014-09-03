using NetAspect.Weaver.Tests.docs.MethodPossibilities.Before;

namespace NetAspect.Weaver.Tests.docs.MethodPossibilities
{
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
   [InterceptorDescription("OnFinallyMethodForParameter", "after the constructor is executed when an exception occured or not")]
   public class DocConfiguration
   {
   }
}
