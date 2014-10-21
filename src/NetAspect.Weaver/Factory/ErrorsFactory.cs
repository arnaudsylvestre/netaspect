using System.Collections.Generic;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Factory
{
    public class ErrorsFactory
    {
        public static Dictionary<ErrorCode, ErrorInfo> CreateAvailableErrors()
        {
            return new Dictionary<ErrorCode, ErrorInfo>
               {
                  {ErrorCode.ImpossibleToOutTheParameter, new ErrorInfo("impossible to out the parameter '{0}' in the method {1} of the type '{2}'")},
                  {ErrorCode.ImpossibleToReferenceTheParameter, new ErrorInfo("impossible to ref/out the parameter '{0}' in the method {1} of the type '{2}'")},
                  {ErrorCode.ParameterWithBadType, new ErrorInfo("the {0} parameter in the method {1} of the type '{2}' is declared with the type '{3}' but it is expected to be {4}")},
                  {ErrorCode.SelectorMustBeStatic, new ErrorInfo("The selector {0} in the aspect {1} must be static")},
                  {ErrorCode.NoDebuggingInformationAvailable, new ErrorInfo("The parameter {0} in method {1} of type {2} will have the default value because there is no debugging information")},
                  {
                     ErrorCode.ParameterWithBadTypeBecauseReturnMethod,
                     new ErrorInfo("the {0} parameter in the method {1} of the type '{2}' is declared with the type '{3}' but it is expected to be {4} because the return type of the method {5} in the type {6}")
                  },
                  {ErrorCode.MustNotBeVoid, new ErrorInfo("Impossible to use the {0} parameter in the method {1} of the type '{2}' because the return type of the method {3} in the type {4} is void")},
                  {ErrorCode.ImpossibleToRefGenericParameter, new ErrorInfo("Impossible to ref a generic parameter")},
                  {ErrorCode.ParameterCanNotBeUsedInStaticMethod, new ErrorInfo("the {0} parameter can not be used for static method interceptors")},
                  {ErrorCode.UnknownParameter, new ErrorInfo("The parameter '{0}' is unknown. Expected one of : {1}")},
                  {ErrorCode.NotAvailableInStaticStruct, new ErrorInfo("the {0} parameter in the method {1} of the type '{2}' is not available for static member in struct")},
                  {ErrorCode.NotAvailableInStatic, new ErrorInfo(ErrorLevel.Warning, "the {0} parameter in the method {1} of the type '{2}' is not available for static member : default value will be passed")},
                  {ErrorCode.ParameterAlreadyDeclared, new ErrorInfo("The parameter {0} is already declared")},
                  {ErrorCode.SelectorMustReturnBooleanValue, new ErrorInfo("The selector {0} in the aspect {1} must return boolean value")},
                  {ErrorCode.SelectorBadParameterType, new ErrorInfo("The parameter {0} in the method {1} of the aspect {2} is expected to be {3}")},
                  {ErrorCode.SelectorBadParameterName, new ErrorInfo("The parameter {0} in the method {1} of the aspect {2} is unexpected. '{3}' parameter must be used")},
                  {ErrorCode.InterceptorMustBeVoid, new ErrorInfo("The {0} interceptor in the {1} aspect must be void")},
                  {ErrorCode.TooManySelectorsWithSameName, new ErrorInfo("Only one {0} can be defined in the aspect {1}")},
                  {ErrorCode.SelectorMustHaveParameters, new ErrorInfo("The selector {0} of the aspect {1} must have the following parameter : {2}")},
                  {ErrorCode.AssemblyGeneratedIsNotCompliant, new ErrorInfo(ErrorLevel.Failure, "An internal error : {0}")},
                  {ErrorCode.ImpossibleToHavePerInstanceLifeCycleInStaticMethod, new ErrorInfo("Impossible to use the aspect {0} in the method {1} of type {2} because this method is static and the aspect is declared as PerInstance")},
                  {ErrorCode.AttributeTypeNotAllowed, new ErrorInfo("The parameter '{0}' in a constructor of the aspect {1} can not be declared with the type {2}. Only the following types are allowed : {3}")},
                  {ErrorCode.AspectWithSelectorMustHaveDefaultConstructor, new ErrorInfo("The aspect '{0}' must have a constructor with no parameters because he got selectors")},
               };
        }
    }
}