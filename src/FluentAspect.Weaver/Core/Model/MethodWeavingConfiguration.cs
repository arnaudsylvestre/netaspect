using System;

namespace FluentAspect.Weaver.Core
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

      public InterceptorInformation Before
      {
         get
         {
            return new InterceptorInformation(_attribute.GetInterceptorMethod("Before"));
         }
      }

      public InterceptorInformation After
      {
         get
         {
            return new InterceptorInformation(_attribute.GetInterceptorMethod("After"));
         }
      }

      public InterceptorInformation OnException
      {
         get
         {
            return new InterceptorInformation(_attribute.GetInterceptorMethod("OnException"));
         }
      }
   }
}