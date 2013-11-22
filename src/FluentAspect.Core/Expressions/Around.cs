using System;

namespace FluentAspect.Core.Expressions
{
    public interface IInterceptor
    {
        void Before();
        void After(Around.MethodCallResult ret);
        void OnException(Exception e);
    }

    public class Around
    {
        public class MethodCallResult
        {
            public object Result { get; set; }
        }

        public static object Call(object @this, string methodName, object[] args, IInterceptor interceptor)
        {
            try
            {
                interceptor.Before();
                var ret = @this.GetType().GetMethod(methodName).Invoke(@this, args);
                var result = new MethodCallResult()
                    {
                        Result = ret
                    };
                interceptor.After(result);
                return result.Result;
            }
            catch (Exception e)
            {
                interceptor.OnException(e);
                throw;
            }
        }
    }
}