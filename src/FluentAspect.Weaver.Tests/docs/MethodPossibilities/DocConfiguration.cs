namespace NetAspect.Weaver.Tests.docs.MethodPossibilities.Before
{
   [ParameterDescription("instance", "The instance")]
   [ParameterDescription("method", "The method")]
   [ParameterDescription("parameters", "The parameters")]
   [ParameterDescription("parameter name", "The parameter's name")]
   [ParameterDescription("result", "The result")]
   [ParameterDescription("exception", "The exception raised")]


   [InterceptorDescription("Before", "before the method is executed")]
   [InterceptorDescription("After", "after the method is executed")]
   [InterceptorDescription("OnException", "when an exception occured in the method")]
   [InterceptorDescription("OnFinally", "after the method is executed when an exception occured or not")]


   [PossibilityDocumentation("MethodWeaving", "PossibilityDescription", "On methods")]
   [PossibilityDocumentation("ConstructorWeaving", "PossibilityDescription", "On constructors")]

   public class DocConfiguration
   {
      
   }
}