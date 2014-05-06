using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Weaver.Checkers;

namespace NetAspect.Weaver.Core.Weaver.Generators
{
   public class InterceptorParameterConfiguration<T>
   {
      public class MyInterceptorParameterChecker : IInterceptorParameterChecker
      {
         public List<Action<ParameterInfo, ErrorHandler>> Checkers = new List<Action<ParameterInfo, ErrorHandler>>();

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
      }


      public class MyGenerator : IInterceptorParameterIlGenerator<T>
      {
         public List<Action<ParameterInfo, List<Instruction>, T>> Generators = new List<Action<ParameterInfo, List<Instruction>, T>>();

         public void GenerateIl(ParameterInfo parameterInfo, List<Instruction> instructions, T info)
         {
            foreach (var generator in Generators)
            {
               generator(parameterInfo, instructions, info);
            }
         }
      }

      public InterceptorParameterConfiguration(string name)
      {
         Name = name;
         Generator = new MyGenerator();
         Checker = new MyInterceptorParameterChecker();
      }

      public MyGenerator Generator { get; private set; }
      public MyInterceptorParameterChecker Checker { get; private set; }
      public string Name;
   }
}