using System.Collections.Generic;
using FluentAspect.Weaver.Core;
using FluentAspect.Weaver.Core.WeaverBuilders;
using Mono.Cecil;

namespace FluentAspect.Weaver.Helpers
{
    class MethodDefinitionAdapter : IMethod
    {
        private MethodReference method;

        public MethodDefinitionAdapter(MethodReference method_P)
        {
            method = method_P;
        }

        public string Name
        {
            get { return method.Name; }
        }
        public IType DeclaringType
        {
            get { return new TypeDefinitionAdapter(method.DeclaringType); }
        }

       public IEnumerable<IParameter> Parameters { get
       {
          List<IParameter> parameters_L = new List<IParameter>();

          foreach (var parameterDefinition_L in method.Parameters)
          {
             parameters_L.Add(new ParameterDefinitionAdapter(parameterDefinition_L));
          }
          return parameters_L;
       } }
    }

   internal class ParameterDefinitionAdapter : IParameter
   {
      private readonly ParameterDefinition _parameterDefinition;

      public ParameterDefinitionAdapter(ParameterDefinition parameterDefinition_P)
      {
         _parameterDefinition = parameterDefinition_P;
      }

      public IType Type { get
      {
         return new TypeDefinitionAdapter(_parameterDefinition.ParameterType);
      } }

      public string Name {
         get { return _parameterDefinition.Name; }
      }
   }
}