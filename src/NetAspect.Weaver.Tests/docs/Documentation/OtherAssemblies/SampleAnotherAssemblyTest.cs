using System;
using System.Collections.Generic;
using System.Reflection;
using NetAspect.Sample.Dep;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.Selectors.OtherAssemblies
{
   public class SampleAnotherAssemblyTest :
      NetAspectTest<SampleAnotherAssemblyTest.MyAspectAttribute, DepClassWhichCallField>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
             Assert.IsNull(MyAspectAttribute.Caller);
            var classToWeave_L = new DepClassWhichCallField();
            classToWeave_L.CallField("value");
            Assert.AreEqual(classToWeave_L, MyAspectAttribute.Caller);
         };
      }

       public class ClassToWeave
       {
           // Defined in another assembly
       }

      public class MyAspectAttribute : Attribute
      {
         public static DepClassWhichCallField Caller;


         public static IEnumerable<Assembly> AssembliesToWeave = new List<Assembly> {typeof (DepClassWhichCallField).Assembly};
         public bool NetAspectAttribute = true;

         public void BeforeUpdateField(DepClassWhichCallField callerInstance)
         {
            Caller = callerInstance;
         }

         public static bool SelectField(FieldInfo field)
         {
            return field.Name == "Field";
         }
      }
   }
}
