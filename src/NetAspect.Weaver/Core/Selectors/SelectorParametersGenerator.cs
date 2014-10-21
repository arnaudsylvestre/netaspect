using System;
using System.Linq;
using System.Reflection;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Core.Selectors
{
   public class SelectorParametersGenerator<T>
   {
      
       private string parameterName;
       private Func<T, object> provider;
       private Type Type;

       public SelectorParametersGenerator(string parameterName, Func<T, object> valueProvider, Type type)
       {
           this.parameterName = parameterName;
           provider = valueProvider;
           Type = type;
       }

       public object Generate(T data)
      {
          return provider(data);
      }

       public void Check(MethodInfo method, ErrorHandler errorHandler)
       {
           EnsureOneParameterInSelector(method, errorHandler);
           if (errorHandler.Errors.Any())
               return;
           EnsureParameterNameIsAsExpected(method, errorHandler);
           EnsureParameterTypeIsAsExpected(method, errorHandler);
       }

       private void EnsureParameterTypeIsAsExpected(MethodInfo method, ErrorHandler errorHandler)
       {
           var parameterInfo = method.GetParameters().First();
           if (Type != parameterInfo.ParameterType)
               errorHandler.OnError(ErrorCode.SelectorBadParameterType, FileLocation.None, parameterInfo.Name,
                                    method.Name, method.DeclaringType.FullName, Type);
       }

       private void EnsureParameterNameIsAsExpected(MethodInfo method, ErrorHandler errorHandler)
       {
           var parameterInfo = method.GetParameters().First();
           if (parameterName != parameterInfo.Name.ToLower())
           {
               errorHandler.OnError(ErrorCode.SelectorBadParameterName, FileLocation.None, parameterInfo.Name,
                                    method.Name, method.DeclaringType.FullName, this.parameterName);
           }
       }

       private void EnsureOneParameterInSelector(MethodInfo method, ErrorHandler errorHandler)
       {
           if (method.GetParameters().Length != 1)
           {
               errorHandler.OnError(ErrorCode.SelectorMustHaveParameters, FileLocation.None, method.Name,
                                    method.DeclaringType.FullName, this.parameterName);
           }
       }
   }
}
