using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Weaver.Engine;

namespace NetAspect.Weaver.Core.Weaver.Checkers.Aspects
{
    public class MultiAspectChecker : WeavingModelComputer.IAspectChecker
    {
        private List<WeavingModelComputer.IAspectChecker> aspectCheckers;

        public MultiAspectChecker(List<WeavingModelComputer.IAspectChecker> aspectCheckers)
        {
            this.aspectCheckers = aspectCheckers;
        }

        public void Check(NetAspectDefinition aspect_P, ErrorHandler errorHandler_P)
        {
            foreach (var aspectChecker in aspectCheckers)
            {
                aspectChecker.Check(aspect_P, errorHandler_P);
            }
        }
    }
}