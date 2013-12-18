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

       [Thrower]
       public MyClassToWeaveWithAttributes(bool thrown)
       {
          this.thrown = thrown;
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

      [CheckBefore]
      public string CheckBeforeWithAttributes(BeforeParameter parameter)
      {
          return parameter.Value;
      }

      [CheckBefore]
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


      [CheckBefore]
      private string CheckBeforeWithAttributesProtected(BeforeParameter parameter)
      {
          return parameter.Value;
      }

       public string CallCheckBeforeWithAttributesProtected(BeforeParameter beforeParameter)
       {
           return CheckBeforeWithAttributesProtected(beforeParameter);
       }
   }
}