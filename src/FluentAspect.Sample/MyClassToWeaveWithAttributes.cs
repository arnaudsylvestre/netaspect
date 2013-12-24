using System;
using FluentAspect.Sample.Attributes;

namespace FluentAspect.Sample
{
   public class MyClassToWeaveWithAttributes
   {
       //public MyClassToWeaveWithAttributes()
       //{

       //}
      private bool thrown = false;

       [ThrowerNetAspect]
       public MyClassToWeaveWithAttributes(bool thrown)
       {
          this.thrown = thrown;
       }

       public string PropertyGetter
       {
           [GetPropertyNetAspectAttribute]
           get { return "1"; }
       }

       public string CheckWithReturn()
      {
         return "NotWeaved";
      }

      public string CheckWithParameters(string aspectWillReturnThis)
      {
         return "NotWeaved";
      }

      public void CheckWithVoid()
      {

      }

      public string CheckWithGenerics<T>(T arg)
      {
         return arg.ToString() + "<>" + typeof(T).FullName;
      }

      public void CheckThrow()
      {
         throw new NotImplementedException();
      }

      [CheckBeforeNetAspect]
      public string CheckBeforeWithAttributes(BeforeParameter parameter)
      {
          return parameter.Value;
      }

      [CheckBeforeNetAspect]
      private string CheckBeforeWithAttributesPrivate(BeforeParameter parameter)
      {
          return parameter.Value;
      }
      public string CallCheckBeforeWithAttributesPrivate(BeforeParameter parameter)
      {
          return CheckBeforeWithAttributesPrivate(parameter);
      }

      public static string CheckStatic(BeforeParameter parameter)
      {
         return parameter.Value;
      }

      public string CheckNotRenameInAssembly()
      {
         return CheckWithReturn();
      }


      [CheckBeforeNetAspect]
      private string CheckBeforeWithAttributesProtected(BeforeParameter parameter)
      {
          return parameter.Value;
      }

       public string CallCheckBeforeWithAttributesProtected(BeforeParameter beforeParameter)
       {
           return CheckBeforeWithAttributesProtected(beforeParameter);
       }
   }


   public class GetPropertyNetAspectAttribute : Attribute
   {
       public void After(ref string result)
       {
           result = "3";
       }

   }
}