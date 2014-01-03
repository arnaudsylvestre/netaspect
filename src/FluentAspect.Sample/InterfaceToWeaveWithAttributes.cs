using FluentAspect.Sample.AOP;

namespace FluentAspect.Sample
{
    public interface InterfaceToWeaveWithAttributes
    {
        [CheckBeforeAspect]
        string CheckBeforeWithAttributes(BeforeParameter parameter);
    }
}