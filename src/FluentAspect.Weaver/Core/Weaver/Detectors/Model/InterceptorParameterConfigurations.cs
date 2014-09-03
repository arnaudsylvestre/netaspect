using System.Collections.Generic;

namespace NetAspect.Weaver.Core.Weaver.Detectors.Model
{
   public class InterceptorParameterConfigurations
   {
      private readonly List<InterceptorParameterConfiguration> possibleParameters = new List<InterceptorParameterConfiguration>();

      public IEnumerable<InterceptorParameterConfiguration> PossibleParameters
      {
         get { return possibleParameters; }
      }


      public InterceptorParameterConfiguration AddPossibleParameter(string parameterName)
      {
         return Create(parameterName);
      }

      public InterceptorParameterConfiguration Create(string parameterName)
      {
         var item = new InterceptorParameterConfiguration(parameterName);
         possibleParameters.Add(item);
         return item;
      }
   }
}
