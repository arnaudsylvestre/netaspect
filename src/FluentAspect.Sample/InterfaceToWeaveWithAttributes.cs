using System;
using FluentAspect.Sample.Attributes;

namespace FluentAspect.Sample
{
   public interface InterfaceToWeaveWithAttributes
   {
       [CheckBeforeAspect]
       string CheckBeforeWithAttributes(BeforeParameter parameter);

   }
}