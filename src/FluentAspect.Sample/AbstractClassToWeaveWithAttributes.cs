using NetAspect.Sample.AOP;

namespace NetAspect.Sample
{
    public abstract class AbstractClassToWeaveWithAttributes
    {
        [CheckBeforeAspect]
        public abstract string CheckBeforeWithAttributes(BeforeParameter parameter);
    }
}