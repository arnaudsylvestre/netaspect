using System;

namespace FluentAspect.Weaver.Core.Model
{
   public class MethodWeavingConfiguration
   {
      private readonly object _attribute;

      public Type Type
      {
         get { return _attribute.GetType(); }
      }

      public MethodWeavingConfiguration(object attribute_P)
      {
         _attribute = attribute_P;
      }

      public Interceptor Before
      {
         get
         {
            return new Interceptor(_attribute.GetInterceptorMethod("Before"));
         }
      }

      public Interceptor After
      {
         get
         {
            return new Interceptor(_attribute.GetInterceptorMethod("After"));
         }
      }

      public Interceptor OnException
      {
         get
         {
            return new Interceptor(_attribute.GetInterceptorMethod("OnException"));
         }
      }
   }
}