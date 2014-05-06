using System.Collections.Generic;
using NetAspect.Weaver.Core.Weaver.Generators;

namespace NetAspect.Weaver.Core.Weaver.Detectors.Model
{
   public class InterceptorParameterConfigurations<T>
   {
      readonly List<InterceptorParameterConfiguration<T>> possibleParameters = new List<InterceptorParameterConfiguration<T>>();

      public IEnumerable<InterceptorParameterConfiguration<T>> PossibleParameters
      {
         get { return possibleParameters; }
      }

      public InterceptorParameterConfiguration<T> Create(string parameterName)
      {
         InterceptorParameterConfiguration<T> item = new InterceptorParameterConfiguration<T>(parameterName);
         possibleParameters.Add(item);
         return item;
      }
   }
}