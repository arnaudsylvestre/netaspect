using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;
using NetAspect.Weaver.Core.Weaver.ToSort.Engine.Instructions;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Engine
{
    public static class AspectInstanceForInstructionExtensions
    {



        public static void Check(this AspectInstanceForInstruction instance, ErrorHandler errorHandler, VariablesForInstruction variables, Variable aspectInstance)
        {
            instance.Before.Check(errorHandler, variables, aspectInstance);
            instance.After.Check(errorHandler, variables, aspectInstance);
        }

        public static void Inject(this AspectInstanceForInstruction instance, AroundInstructionIl il, VariablesForInstruction variables, Variable aspectInstance)
        {
            instance.Before.Inject(il.BeforeInstruction, variables, aspectInstance);
            instance.After.Inject(il.AfterInstruction, variables, aspectInstance);
        }
    }
}