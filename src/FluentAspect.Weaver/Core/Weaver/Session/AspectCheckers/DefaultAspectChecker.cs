using System.Reflection;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Errors;
using NetAspect.Weaver.Core.Selectors;
using NetAspect.Weaver.Core.Weaver.Engine;

namespace NetAspect.Weaver.Core.Weaver.Checkers.Aspects
{
    public class DefaultAspectChecker : WeavingSessionComputer.IAspectChecker
   {
      public void Check(NetAspectDefinition aspect, ErrorHandler errorHandler)
      {
         EnsureSelector(aspect.FieldSelector, errorHandler, aspect);
         EnsureSelector(aspect.MethodSelector, errorHandler, aspect);
         EnsureSelector(aspect.ConstructorSelector, errorHandler, aspect);
         EnsureSelector(aspect.PropertySelector, errorHandler, aspect);
         EnsureSelector(aspect.ParameterSelector, errorHandler, aspect);
      }

      private void EnsureSelector<T>(Selector<T> fieldSelector, ErrorHandler errorHandler, NetAspectDefinition aspectP)
      {
         try
         {
            fieldSelector.Check(errorHandler);
         }
         catch (AmbiguousMatchException e)
         {
            errorHandler.OnError(ErrorCode.TooManySelectorsWithSameName, FileLocation.None, fieldSelector.SelectorName, aspectP.Type.FullName);
         }
      }
   }
}
