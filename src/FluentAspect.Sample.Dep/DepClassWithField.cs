using System.Reflection;

namespace NetAspect.Sample.Dep
{
   public class DepClassWithField
   {
      public string Field;

      public void Test()
      {
         ParameterInfo toto = MethodBase.GetCurrentMethod().GetParameters()[3];
          var toto2 = typeof (DepClassWithProperty);
      }
   }

   public class DepClassWithProperty
   {
      public string Property { get; set; }
   }
}
