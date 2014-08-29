using System;
using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Errors;
using NetAspect.Weaver.Core.Weaver.Detectors.Engine.Selectors;

namespace NetAspect.Weaver.Core.Weaver.Engine.AspectCheckers
{
   public class DefaultAspectChecker : WeavingModelComputer.IAspectChecker
   {
      public void Check(NetAspectDefinition aspect_P, ErrorHandler errorHandler_P)
       {
           Ensure(aspect_P.FieldSelector, errorHandler_P, aspect_P);
           Ensure(aspect_P.MethodSelector, errorHandler_P, aspect_P);
           Ensure(aspect_P.ConstructorSelector, errorHandler_P, aspect_P);
           Ensure(aspect_P.PropertySelector, errorHandler_P, aspect_P);
           Ensure(aspect_P.ParameterSelector, errorHandler_P, aspect_P);
      }

       private void Ensure<T>(Selector<T> fieldSelector, ErrorHandler errorHandler, NetAspectDefinition aspectP)
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