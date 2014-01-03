namespace FluentAspect.Weaver.Core.Model
{
   public class CallWeavingConfiguration
   {
      private readonly object _attribute;

      public CallWeavingConfiguration(object attribute_P)
      {
         _attribute = attribute_P;
      }

      public Interceptor BeforeInterceptor
      {
         get
         {
            return new Interceptor(_attribute.GetInterceptorMethod("BeforeCall"));
         }
      }

      public Interceptor AfterInterceptor
      {
         get
         {
            return new Interceptor(_attribute.GetInterceptorMethod("AfterCall"));
         }
      }
   }
}