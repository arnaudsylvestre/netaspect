using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
          EnsureNotDifferentInterceptorsInOneAspect(aspect, errorHandler);
      }

        private void EnsureNotDifferentInterceptorsInOneAspect(NetAspectDefinition aspect, ErrorHandler errorHandler)
        {
            var compliants = new List<List<string>>
                {
                    new List<Expression<Func<Interceptor>>>()
                        {
                            () => aspect.BeforeMethod,
                            () => aspect.AfterMethod,
                            () => aspect.OnFinallyMethod,
                            () => aspect.OnExceptionMethod,
                        }.Select(ExtractMethodName).ToList(),
                    new List<Expression<Func<Interceptor>>>()
                        {
                            () => aspect.BeforeConstructor,
                            () => aspect.AfterConstructor,
                            () => aspect.OnFinallyConstructor,
                            () => aspect.OnExceptionConstructor,
                        }.Select(ExtractMethodName).ToList(),
                    new List<Expression<Func<Interceptor>>>()
                        {
                            () => aspect.BeforePropertyGetMethod,
                            () => aspect.AfterPropertyGetMethod,
                            () => aspect.OnFinallyPropertyGetMethod,
                            () => aspect.OnExceptionPropertyGetMethod,
                        }.Select(ExtractMethodName).ToList(),
                    new List<Expression<Func<Interceptor>>>()
                        {
                            () => aspect.BeforePropertySetMethod,
                            () => aspect.AfterPropertySetMethod,
                            () => aspect.OnFinallyPropertySetMethod,
                            () => aspect.OnExceptionPropertySetMethod,
                        }.Select(ExtractMethodName).ToList(),
                    new List<Expression<Func<Interceptor>>>()
                        {
                            () => aspect.BeforeMethodForParameter,
                            () => aspect.AfterMethodForParameter,
                            () => aspect.OnFinallyMethodForParameter,
                            () => aspect.OnExceptionMethodForParameter,
                        }.Select(ExtractMethodName).ToList(),
                    new List<Expression<Func<Interceptor>>>()
                        {
                            () => aspect.BeforeConstructorForParameter,
                            () => aspect.AfterConstructorForParameter,
                            () => aspect.OnFinallyConstructorForParameter,
                            () => aspect.OnExceptionConstructorForParameter,
                        }.Select(ExtractMethodName).ToList(),
                    new List<Expression<Func<Interceptor>>>()
                        {
                            () => aspect.BeforeCallMethod,
                            () => aspect.AfterCallMethod,
                        }.Select(ExtractMethodName).ToList(),
                    new List<Expression<Func<Interceptor>>>()
                        {
                            () => aspect.BeforeCallConstructor,
                            () => aspect.AfterCallConstructor,
                        }.Select(ExtractMethodName).ToList(),
                    new List<Expression<Func<Interceptor>>>()
                        {
                            () => aspect.AfterGetField,
                            () => aspect.BeforeGetField,
                        }.Select(ExtractMethodName).ToList(),
                    new List<Expression<Func<Interceptor>>>()
                        {
                            () => aspect.AfterUpdateField,
                            () => aspect.BeforeUpdateField,
                        }.Select(ExtractMethodName).ToList(),
                    new List<Expression<Func<Interceptor>>>()
                        {
                            () => aspect.AfterGetProperty,
                            () => aspect.BeforeGetProperty,
                        }.Select(ExtractMethodName).ToList(),
                    new List<Expression<Func<Interceptor>>>()
                        {
                            () => aspect.AfterSetProperty,
                            () => aspect.BeforeSetProperty,
                        }.Select(ExtractMethodName).ToList(),
                };

            var interceptors = new List<string>();
            var propertyInfos = aspect.GetType().GetProperties().Where(p => p.PropertyType == typeof (Interceptor));
            foreach (var propertyInfo in propertyInfos)
            {
                if (((Interceptor)propertyInfo.GetValue(aspect, new object[0])).Method != null)
                    interceptors.Add(propertyInfo.Name);
            }
            var firstOrDefault = interceptors.FirstOrDefault();
            if (firstOrDefault == null)
                return;
            foreach (var compliant in compliants)
            {
                if (compliant.Contains(firstOrDefault))
                {
                    foreach (var interceptor in interceptors)
                    {
                        if (!compliant.Contains(interceptor))
                        {
                            errorHandler.OnError(ErrorCode.AllInterceptorsMustHaveTheSameScope, FileLocation.None, firstOrDefault, interceptor, aspect.Type.FullName);
                            return;
                        }
                    }
                    return;
                }
            }
        }

        private string ExtractMethodName(Expression<Func<Interceptor>> param)
        {
            var expression = param.Body as MemberExpression;
            return expression.Member.Name;
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
         catch (AmbiguousMatchException e)
         {
            errorHandler.OnError(ErrorCode.TooManySelectorsWithSameName, FileLocation.None, fieldSelector.SelectorName, aspectP.Type.FullName);
         }
      }
   }
}
