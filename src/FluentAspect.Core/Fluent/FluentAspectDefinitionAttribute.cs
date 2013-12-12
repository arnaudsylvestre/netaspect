﻿using System;
using System.Collections.Generic;
using System.Linq;
using FluentAspect.Core.Methods;

namespace FluentAspect.Core
{
    public class MethodMatch
    {
        public Func<IMethod, bool> Matcher { get; set; }

        public Type AdviceName { get; set; }
    }
    public class ConstructorMatch
    {
        public Func<IMethod, bool> Matcher { get; set; }

        public Type AdviceName { get; set; }
    }

    public abstract class FluentAspectDefinition
    {
       

       public List<MethodMatch> methodMatches = new List<MethodMatch>(); 


       public abstract void Setup();

         protected void WeaveMethodWhichMatches<T>(Func<IMethod, bool> methodMatcher)
            where T : new()
         {
            methodMatches.Add(new MethodMatch() { Matcher = methodMatcher, AdviceName = typeof(T) });
         }
    }
}