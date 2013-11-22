using System;
using FluentAspect.Core.Core;

namespace FluentAspect.Core.Expressions
{
   public class Around
    {
        public static object Call(object @this, string methodName, object[] args, IInterceptor interceptor)
        {
               var call = new MethodCall(@this, methodName, args);
            try
            {
               interceptor.Before(call);
                var ret = @this.GetType().GetMethod(methodName).Invoke(@this, args);
                var result = new MethodCallResult()
                    {
                        Result = ret
                    };
                interceptor.After(call, result);
                return result.Result;
            }
            catch (Exception e)
            {
               interceptor.OnException(call, e);
                throw;
            }
        }
    }
}