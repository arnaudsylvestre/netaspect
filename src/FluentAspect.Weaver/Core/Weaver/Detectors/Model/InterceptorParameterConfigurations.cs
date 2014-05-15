using System.Collections.Generic;

namespace NetAspect.Weaver.Core.Weaver.Detectors.Model
{
   public class InterceptorParameterConfigurations
   {
      readonly List<InterceptorParameterConfiguration> possibleParameters = new List<InterceptorParameterConfiguration>();

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
         InterceptorParameterConfiguration item = new InterceptorParameterConfiguration(parameterName);
         possibleParameters.Add(item);
         return item;
      }
   }
}