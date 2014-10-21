using System.Collections.Generic;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;

namespace NetAspect.Weaver.Core.Weaver.Engine.InterceptorParameters
{
   public class InterceptorParameterPossibilities<T> where T : VariablesForMethod
   {
       private readonly List<InterceptorParameterPossibility<T>> possibleParameters = new List<InterceptorParameterPossibility<T>>();

       public IEnumerable<InterceptorParameterPossibility<T>> PossibleParameters
      {
         get { return possibleParameters; }
      }


       public InterceptorParameterPossibility<T> AddPossibleParameter(string parameterName)
      {
           var item = new InterceptorParameterPossibility<T>(parameterName);
           possibleParameters.Add(item);
           return item;
      }
   }
}
