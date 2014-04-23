using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NetAspect.Weaver.Core.Model.Aspect;

namespace NetAspect.Weaver.Core.Weaver.Engine.AspectFinders
{
   public class DefaultAspectFinder : WeavingModelComputer.IAspectFinder
   {
      public List<NetAspectDefinition> Find(IEnumerable<Type> types_P)
      {
         return types_P.
             Select(t => new NetAspectDefinition(t)).
             Where(t => IsValid(t))
                       .ToList();
      }

      public bool IsValid(NetAspectDefinition aspect)
      {
         return aspect.Type.GetField("NetAspectAttribute",
                                 BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static |
                                 BindingFlags.Instance) != null;
      }
   }
}