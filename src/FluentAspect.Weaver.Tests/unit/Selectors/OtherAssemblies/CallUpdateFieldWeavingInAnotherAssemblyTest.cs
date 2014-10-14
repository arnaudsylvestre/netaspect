using System;
using System.Collections.Generic;
using System.Reflection;
using NetAspect.Sample.Dep;
using NUnit.Framework;

namespace NetAspect.Weaver.Tests.unit.Selectors.OtherAssemblies
{
   public class CallUpdateFieldWeavingInAnotherAssemblyTest :
      NetAspectTest<CallUpdateFieldWeavingInAnotherAssemblyTest.MyAspect, DepClassWhichCallField>
   {
      protected override Action CreateEnsure()
      {
         return () =>
         {
            Assert.IsNull(MyAspect.Caller);
            var classToWeave_L = new DepClassWhichCallField();
            classToWeave_L.CallField("value");
            Assert.AreEqual(classToWeave_L, MyAspect.Caller);
         };
      }

      public class MyAspect : Attribute
      {
         public static DepClassWhichCallField Caller;


         public static IEnumerable<Assembly> AssembliesToWeave = new List<Assembly> {typeof (DepClassWhichCallField).Assembly};
         public bool NetAspectAttribute = true;

         public void BeforeUpdateField(DepClassWhichCallField caller)
         {
            Caller = caller;
         }

         public static bool SelectField(FieldInfo field)
         {
            return field.Name == "Field";
         }
      }
   }
}
