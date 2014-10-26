using System;
using System.Reflection;

namespace NetAspect.Sample.Dep
{

   public class DepClassWithField
   {
      public string Field;

       public DepClassWithField()
       {
           
       }

       public DepClassWithField(int toto)
       {
           
       }

      public void Test()
      {
          GetType().GetConstructor(BindingFlags.Public, null, new Type[]
              {
                  typeof (int)
              }, null);

      }
   }

   public class DepClassWithProperty
   {
      public string Property { get; set; }
   }
}
