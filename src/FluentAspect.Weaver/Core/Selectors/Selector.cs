using System;
using System.Linq;
using System.Reflection;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Core.Selectors
{
   public class Selector<T>
   {
      private readonly Type _aspect;
      private readonly SelectorParametersGenerator<T> selectorParametersGenerator;

      public Selector(Type aspect, string selectorName, SelectorParametersGenerator<T> selectorParametersGenerator)
      {
         _aspect = aspect;
         SelectorName = selectorName;
         this.selectorParametersGenerator = selectorParametersGenerator;
      }

      public string SelectorName { get; private set; }

      public void Check(ErrorHandler errorHandler)
      {
         MethodInfo method = _aspect.GetMethod(SelectorName);
         if (method == null)
            return;
         selectorParametersGenerator.Check(method, errorHandler);
         if (!method.IsStatic)
            errorHandler.OnError(ErrorCode.SelectorMustBeStatic, FileLocation.None, method.Name, method.DeclaringType.FullName);

         if (method.ReturnType != typeof (bool))
            errorHandler.OnError(ErrorCode.SelectorMustReturnBooleanValue, FileLocation.None, method.Name, method.DeclaringType.FullName);
      }

      public bool IsCompliant(T member)
      {
         try
         {
            MethodInfo method = _aspect.GetMethod(SelectorName);
            if (method == null)
               return false;
            var errorHandler = new ErrorHandler();
            Check(errorHandler);
            if (errorHandler.Errors.Any())
               return false;
            object target = selectorParametersGenerator.Generate(member);
            if (target == null)
               return false;
            return (bool) method.Invoke(null, new[] { target });
         }
         catch (Exception)
         {
            return false;
         }
      }
   }
}
