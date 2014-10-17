using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Weaver.Engine;

namespace NetAspect.Weaver.Core.Weaver.Checkers.Aspects
{
    public class MultiAspectChecker : WeavingSessionComputer.IAspectChecker
    {
        private List<WeavingSessionComputer.IAspectChecker> aspectCheckers;

        public MultiAspectChecker(List<WeavingSessionComputer.IAspectChecker> aspectCheckers)
        {
            this.aspectCheckers = aspectCheckers;
        }

        public void Check(NetAspectDefinition aspect, ErrorHandler errorHandler)
        {
            foreach (var aspectChecker in aspectCheckers)
            {
                aspectChecker.Check(aspect, errorHandler);
            }
        }
    }
}