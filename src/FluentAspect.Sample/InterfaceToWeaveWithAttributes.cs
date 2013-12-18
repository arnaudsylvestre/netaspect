using System;
using FluentAspect.Sample.Attributes;

namespace FluentAspect.Sample
{
   public interface InterfaceToWeaveWithAttributes
   {
       [CheckBefore]
       string CheckBeforeWithAttributes(BeforeParameter parameter);

   }
}