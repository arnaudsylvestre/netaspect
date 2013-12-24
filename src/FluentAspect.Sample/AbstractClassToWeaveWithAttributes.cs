using System;
using FluentAspect.Sample.Attributes;

namespace FluentAspect.Sample
{
   public abstract class AbstractClassToWeaveWithAttributes
   {
       [CheckBeforeAspect]
       public abstract string CheckBeforeWithAttributes(BeforeParameter parameter);

   }
}