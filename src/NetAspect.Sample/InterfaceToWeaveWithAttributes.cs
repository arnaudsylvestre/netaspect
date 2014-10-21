using NetAspect.Sample.AOP;

namespace NetAspect.Sample
{
    public interface InterfaceToWeaveWithAttributes
    {
        [CheckBeforeAspect]
        string CheckBeforeWithAttributes(BeforeParameter parameter);
    }
}