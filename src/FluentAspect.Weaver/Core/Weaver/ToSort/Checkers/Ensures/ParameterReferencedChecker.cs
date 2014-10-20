using System.Reflection;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Checkers.Ensures
{
   public class ParameterReferencedChecker : IChecker
   {
      public enum ReferenceModel
      {
         None,
         Referenced,
         Out,
      }

      public ParameterReferencedChecker(ReferenceModel referenced)
      {
         Referenced = referenced;
      }

      private ReferenceModel Referenced { get; set; }

      public void Check(ParameterInfo parameterInfo, ErrorHandler errorHandler)
      {
         CheckReferenced(parameterInfo, errorHandler);
         CheckOut(parameterInfo, errorHandler);
      }

      public void CheckReferenced(ParameterInfo parameterInfo, ErrorHandler errorHandler)
      {
         if (Referenced != ReferenceModel.None)
            return;
         if (parameterInfo.ParameterType.IsByRef)
         {
            errorHandler.OnError(
               ErrorCode.ImpossibleToReferenceTheParameter,
               FileLocation.None,
               parameterInfo.Name,
               parameterInfo.Member.Name,
               parameterInfo.Member.DeclaringType.FullName);
         }
      }


      public void CheckOut(ParameterInfo parameterInfo, ErrorHandler errorHandler)
      {
         if (Referenced == ReferenceModel.Out)
            return;
         if (parameterInfo.IsOut)
         {
            errorHandler.OnError(
               ErrorCode.ImpossibleToOutTheParameter,
               FileLocation.None,
               parameterInfo.Name,
               parameterInfo.Member.Name,
               parameterInfo.Member.DeclaringType.FullName);
         }
      }
   }
}
