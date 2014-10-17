using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Weaver.Data.Variables;

namespace NetAspect.Weaver.Core.Weaver.Engine.Instructions
{
    public static class AspectInstanceForInstructionExtensions
    {



        public static void Check(this AspectInstanceForInstruction instance, ErrorHandler errorHandler, VariablesForInstruction variables)
        {
            instance.Before.Check(errorHandler, variables);
            instance.After.Check(errorHandler, variables);
        }

        public static void Weave(this AspectInstanceForInstruction instance, AroundInstructionIl il, VariablesForInstruction variables)
        {
            instance.Before.Inject(il.BeforeInstruction, variables);
            instance.After.Inject(il.AfterInstruction, variables);
        }
    }
}