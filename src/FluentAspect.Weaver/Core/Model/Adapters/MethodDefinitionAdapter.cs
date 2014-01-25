using System.Collections.Generic;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.Model.Adapters
{
    internal class MethodDefinitionAdapter : IMethod
    {
        private readonly MethodReference method;

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

        public IEnumerable<IParameter> Parameters
        {
            get
            {
                var parameters_L = new List<IParameter>();

                foreach (ParameterDefinition parameterDefinition_L in method.Parameters)
                {
                    parameters_L.Add(new ParameterDefinitionAdapter(parameterDefinition_L));
                }
                return parameters_L;
            }
        }
    }

    internal class PropertyDefinitionAdapter : IProperty
    {
        private readonly PropertyReference method;

        public PropertyDefinitionAdapter(PropertyReference method_P)
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

        public IEnumerable<IParameter> Indices
        {
            get
            {
                var parameters_L = new List<IParameter>();

                foreach (ParameterDefinition parameterDefinition_L in method.Parameters)
                {
                    parameters_L.Add(new ParameterDefinitionAdapter(parameterDefinition_L));
                }
                return parameters_L;
            }
        }
    }
}