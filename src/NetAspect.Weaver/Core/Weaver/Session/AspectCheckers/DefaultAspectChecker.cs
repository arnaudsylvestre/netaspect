using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Errors;
using NetAspect.Weaver.Core.Selectors;
using NetAspect.Weaver.Helpers.Mono.Cecil.IL;
using NetAspect.Weaver.Helpers.NetFramework;

namespace NetAspect.Weaver.Core.Weaver.Session.AspectCheckers
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
         EnsureAttributeConstructorTypeIsAllowed(aspect, InstructionsExtensions.adders.Keys.ToList(), errorHandler);
          EnsureAspectWithSelectorHasDefaultConstructor(aspect, errorHandler);
          //EnsureNotDifferentInterceptorsInOneAspect(aspect, errorHandler);
      }

        private void EnsureAspectWithSelectorHasDefaultConstructor(NetAspectDefinition aspect, ErrorHandler errorHandler)
        {
            ConstructorInfo constructor = aspect.Type.GetConstructor(new Type[0]);
            if (constructor != null)
                return;
            if (aspect.ConstructorSelector.Exists ||
                aspect.FieldSelector.Exists ||
                aspect.MethodSelector.Exists ||
                aspect.ParameterSelector.Exists ||
                aspect.PropertySelector.Exists)
            {
                errorHandler.OnError(ErrorCode.AspectWithSelectorMustHaveDefaultConstructor, FileLocation.None, aspect.Type.FullName);
            }
        }

        private void EnsureAttributeConstructorTypeIsAllowed(NetAspectDefinition aspect, List<Type> allowedTypes, ErrorHandler errorHandler)
        {
            foreach (var constructor in aspect.Type.GetConstructors(ObjectExtensions.BINDING_FLAGS))
            {
                ParameterInfo[] parameterInfos = constructor.GetParameters();
                foreach (var parameterInfo in parameterInfos)
                {
                    if (!allowedTypes.Contains(parameterInfo.ParameterType))
                        errorHandler.OnError(ErrorCode.AttributeTypeNotAllowed, FileLocation.None, parameterInfo.Name, aspect.Type.FullName, parameterInfo.ParameterType.FullName, string.Join(", ", allowedTypes.Select(t => t.FullName).ToArray()));
                }
            }
        }

        private void EnsureSelector<T>(Selector<T> fieldSelector, ErrorHandler errorHandler, NetAspectDefinition aspectP)
      {
         try
         {
            fieldSelector.Check(errorHandler);
         }
         catch (AmbiguousMatchException)
         {
            errorHandler.OnError(ErrorCode.TooManySelectorsWithSameName, FileLocation.None, fieldSelector.SelectorName, aspectP.Type.FullName);
         }
      }
   }
}
