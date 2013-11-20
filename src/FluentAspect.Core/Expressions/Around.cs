using System;

namespace FluentAspect.Core.Expressions
{
    public interface IInterceptor
    {
        void Before();
        void After(object ret);
        void OnException(Exception e);
    }

    public class Around
    {
        public static object Call(object @this, string methodName, object[] args, IInterceptor interceptor)
        {
            try
            {
                interceptor.Before();
                var ret = @this.GetType().GetMethod(methodName).Invoke(@this, args);
                interceptor.After(ret);
                return ret;
            }
            catch (Exception e)
            {
                interceptor.OnException(e);
                throw;
            }
        }
    }
}