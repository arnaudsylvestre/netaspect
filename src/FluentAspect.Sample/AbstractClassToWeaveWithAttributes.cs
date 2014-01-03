using FluentAspect.Sample.AOP;

namespace FluentAspect.Sample
{
    public abstract class AbstractClassToWeaveWithAttributes
    {
        [CheckBeforeAspect]
        public abstract string CheckBeforeWithAttributes(BeforeParameter parameter);
    }
}