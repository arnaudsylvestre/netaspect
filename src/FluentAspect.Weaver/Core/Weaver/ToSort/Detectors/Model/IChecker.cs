using System.Reflection;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Model
{
   public interface IChecker
   {
      void Check(ParameterInfo parameterInfo, ErrorHandler errorHandler);
   }
}
