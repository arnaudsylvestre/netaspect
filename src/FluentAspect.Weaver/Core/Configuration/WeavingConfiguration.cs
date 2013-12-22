using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Core;
using FluentAspect.Core.Methods;

namespace FluentAspect.Weaver.Core
{
    public class MethodMatch
    {
        public Func<IMethod, bool> Matcher { get; set; }

        public List<Type> InterceptorTypes { get; set; }
    }

   public class WeavingConfiguration
   {
      public WeavingConfiguration()
      {
         Methods = new List<MethodMatch>();
         Constructors = new List<MethodMatch>();
      }

      public List<MethodMatch> Methods { get; private set; }

      public List<MethodMatch> Constructors { get; private set; }
   }
}