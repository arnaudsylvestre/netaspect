using System;
using System.Reflection;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Core.Weaver.Detectors.Engine.Selectors
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
         MethodInfo _method = _aspect.GetMethod(SelectorName);
         if (_method == null)
            return;
         selectorParametersGenerator.Check(_method, errorHandler);
         if (!_method.IsStatic)
            errorHandler.OnError(ErrorCode.SelectorMustBeStatic, FileLocation.None, _method.Name, _method.DeclaringType.FullName);

         if (_method.ReturnType != typeof (bool))
            errorHandler.OnError(ErrorCode.SelectorMustReturnBooleanValue, FileLocation.None, _method.Name, _method.DeclaringType.FullName);
      }

      public bool IsCompliant(T member)
      {
         try
         {
            MethodInfo _method = _aspect.GetMethod(SelectorName);
            if (_method == null)
               return false;
            var errorHandler = new ErrorHandler();
            Check(errorHandler);
            if (errorHandler.Errors.Count > 0)
               return false;
            object[] target = selectorParametersGenerator.Generate(_method, member);
            if (target[0] == null)
               return false;
            return (bool) _method.Invoke(null, target);
         }
         catch (Exception)
         {
            return false;
         }
      }
   }
}
