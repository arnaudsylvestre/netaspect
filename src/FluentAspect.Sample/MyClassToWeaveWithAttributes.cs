﻿using System;
using FluentAspect.Sample.Attributes;

namespace FluentAspect.Sample
{
   public class MyClassToWeaveWithAttributes
   {
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

      [CheckBeforeAttribute]
      public string CheckBefore(BeforeParameter parameter)
      {
         return parameter.Value;
      }

      public static string CheckStatic(BeforeParameter parameter)
      {
         return parameter.Value;
      }

      public string CheckNotRenameInAssembly()
      {
         return CheckWithReturn();
      }
   }
}