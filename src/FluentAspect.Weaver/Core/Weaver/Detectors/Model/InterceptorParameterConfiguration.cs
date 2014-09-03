using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.Detectors.Model
{
   public class InterceptorParameterConfiguration
   {
      public string Name;

      public InterceptorParameterConfiguration(string name)
      {
         Name = name;
         Generator = new MyGenerator();
         Checker = new MyInterceptorParameterChecker();
      }

      public MyGenerator Generator { get; private set; }
      public MyInterceptorParameterChecker Checker { get; private set; }

      public class MyGenerator
      {
         public List<Action<ParameterInfo, List<Instruction>, IlInjectorAvailableVariables>> Generators = new List<Action<ParameterInfo, List<Instruction>, IlInjectorAvailableVariables>>();

         public void GenerateIl(ParameterInfo parameterInfo, List<Instruction> instructions, IlInjectorAvailableVariables info)
         {
            foreach (var generator in Generators)
            {
               generator(parameterInfo, instructions, info);
            }
         }
      }

      public class MyInterceptorParameterChecker
      {
         private readonly List<Action<ParameterInfo, ErrorHandler>> Checkers = new List<Action<ParameterInfo, ErrorHandler>>();

         public void Check(ParameterInfo parameter, ErrorHandler errorListener)
         {
            foreach (var checker in Checkers)
            {
               checker(parameter, errorListener);
            }
         }

         public void Add(Action<ParameterInfo, ErrorHandler> action)
         {
            Checkers.Add(action);
         }

         public void Add(IChecker checker)
         {
            Checkers.Add(checker.Check);
         }
      }
   }
}
