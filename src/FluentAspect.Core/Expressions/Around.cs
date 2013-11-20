using System;

namespace FluentAspect.Core.Expressions
{
    public interface IInterceptor
    {
        void Before();
        void After();
        void OnException(Exception e);
    }

    public class Around
    {
        public static object Call(object @this, IInterceptor interceptor)
        {
            try
            {
                interceptor.Before();
                var 
                interceptor.After();
                return null;
            }
            catch (Exception e)
            {
                interceptor.OnException(e);
                throw;
            }
        }
    }
}