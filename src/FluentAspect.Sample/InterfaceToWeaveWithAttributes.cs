using System;
using FluentAspect.Sample.Attributes;

namespace FluentAspect.Sample
{
   public interface InterfaceToWeaveWithAttributes
   {
       [CheckBeforeNetAspect]
       string CheckBeforeWithAttributes(BeforeParameter parameter);

   }
}