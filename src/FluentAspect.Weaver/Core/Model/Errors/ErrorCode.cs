namespace NetAspect.Weaver.Core.Model.Errors
{
   public enum ErrorCode
   {
        ImpossibleToReferenceTheParameter,
        ImpossibleToOutTheParameter,
       ParameterWithBadType,
       AssemblyGeneratedIsNotCompliant,
       NoDebuggingInformationAvailable,
      SelectorMustBeStatic,
      ParameterWithBadTypeBecauseReturnMethod,
      MustNotBeVoid,
      ImpossibleToRefGenericParameter,
      ParameterCanNotBeUsedInStaticMethod,
      UnknownParameter,
      NotAvailableInStaticStruct,
      NotAvailableInStatic,
      ParameterAlreadyDeclared,
      SelectorMustReturnBooleanValue,
      SelectorBadParameterType
   }
}