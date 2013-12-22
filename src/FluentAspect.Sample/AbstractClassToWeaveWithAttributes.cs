using System;
using FluentAspect.Sample.Attributes;

namespace FluentAspect.Sample
{
   public abstract class AbstractClassToWeaveWithAttributes
   {
       [CheckBeforeNetAspect]
       public abstract string CheckBeforeWithAttributes(BeforeParameter parameter);

   }
}