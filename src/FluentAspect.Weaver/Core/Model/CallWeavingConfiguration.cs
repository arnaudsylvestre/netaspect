namespace FluentAspect.Weaver.Core
{
   public class CallWeavingConfiguration
   {
      private readonly object _attribute;

      public CallWeavingConfiguration(object attribute_P)
      {
         _attribute = attribute_P;
      }

      public InterceptorInformation BeforeInterceptor
      {
         get
         {
            return new InterceptorInformation(_attribute.GetInterceptorMethod("BeforeCall"));
         }
      }

      public InterceptorInformation AfterInterceptor
      {
         get
         {
            return new InterceptorInformation(_attribute.GetInterceptorMethod("AfterCall"));
         }
      }
   }
}