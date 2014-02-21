namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory.Parameters
{
    public class InterceptorParametersChecker
    {
        public string ParameterName { get; set; }
        public IInterceptorParameterChecker Checker { get; set; }
    }
}