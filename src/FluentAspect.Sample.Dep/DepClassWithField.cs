using System;
using System.Reflection;

namespace NetAspect.Sample.Dep
{
    public class TotoAttribute : Attribute
    {
        public TotoAttribute(Type t)
        {
            
        }
    }

   public class DepClassWithField
   {
      public string Field;

       [TotoAttribute(typeof(TotoAttribute))]
      public void Test()
      {
         ParameterInfo toto = MethodBase.GetCurrentMethod().GetParameters()[3];
          var toto2 = typeof (DepClassWithProperty);
           Attribute.GetCustomAttributes(MethodBase.GetCurrentMethod(), typeof (TotoAttribute));

      }
   }

   public class DepClassWithProperty
   {
      public string Property { get; set; }
   }
}
