using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Aspect;

namespace NetAspect.Weaver.Core.Weaver.Engine.AspectCheckers
{
   public class DefaultAspectChecker : WeavingModelComputer2.IAspectChecker
   {
      public void Check(NetAspectDefinition aspect_P, ErrorHandler errorHandler_P)
      {
         aspect_P.FieldSelector.Check(errorHandler_P);
         aspect_P.PropertySelector.Check(errorHandler_P);
      }
   }
}