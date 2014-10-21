using System;
using System.Collections.Generic;
using System.Reflection;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;

namespace NetAspect.Weaver.Core.Weaver.Engine.InterceptorParameters
{
    public class InterceptorParameterPossibility<T> where T : VariablesForMethod
    {
        public string Name { get; private set; }
        public List<Action<ParameterInfo, ErrorHandler>> Checkers { get; set; }
        public List<Action<ParameterInfo, List<Mono.Cecil.Cil.Instruction>, T>> Generators { get; set; }


      public InterceptorParameterPossibility(string name)
      {
         Name = name;
          Checkers = new List<Action<ParameterInfo, ErrorHandler>>();
          Generators = new List<Action<ParameterInfo, List<Mono.Cecil.Cil.Instruction>, T>>();
      }
    }
}
