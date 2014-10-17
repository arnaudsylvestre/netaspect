using System.Collections.Generic;
using NetAspect.Weaver.Core.Weaver.Data.Variables;

namespace NetAspect.Weaver.Core.Weaver.Detectors.Model
{
   public class InterceptorParameterConfigurations<T> where T : VariablesForMethod
   {
       private readonly List<InterceptorParameterConfiguration<T>> possibleParameters = new List<InterceptorParameterConfiguration<T>>();

       public IEnumerable<InterceptorParameterConfiguration<T>> PossibleParameters
      {
         get { return possibleParameters; }
      }


       public InterceptorParameterConfiguration<T> AddPossibleParameter(string parameterName)
      {
           var item = new InterceptorParameterConfiguration<T>(parameterName);
           possibleParameters.Add(item);
           return item;
      }
   }
}
