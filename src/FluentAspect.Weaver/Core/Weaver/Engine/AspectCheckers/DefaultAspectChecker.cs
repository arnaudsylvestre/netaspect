using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Core.Weaver.Engine.AspectCheckers
{
   public class DefaultAspectChecker : WeavingModelComputer.IAspectChecker
   {
      public void Check(NetAspectDefinition aspect_P, ErrorHandler errorHandler_P)
      {
         aspect_P.FieldSelector.Check(errorHandler_P);
         aspect_P.PropertySelector.Check(errorHandler_P);
          //EnsureAspectIsAClass(aspect_P, errorHandler_P);
      }

       private void EnsureAspectIsAClass(NetAspectDefinition aspect, ErrorHandler errorHandler)
       {
           if (!aspect.Type.IsClass)
            errorHandler.OnError(ErrorCode.AspectMustBeAClass, FileLocation.None, aspect.Type.FullName);
       }
   }
}