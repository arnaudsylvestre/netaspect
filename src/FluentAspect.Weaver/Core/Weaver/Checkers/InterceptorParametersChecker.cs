namespace FluentAspect.Weaver.Core.V2.Weaver.Checkers
{
    public class InterceptorParametersChecker
    {
        public string ParameterName { get; set; }
        public IInterceptorParameterChecker Checker { get; set; }
    }
}