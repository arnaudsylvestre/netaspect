using System;
using System.Collections.Generic;
using System.Linq;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Helpers.NetFramework;

namespace NetAspect.Weaver.Core.Weaver.Session.AspectFinders
{
   public class DefaultAspectFinder : WeavingSessionComputer.IAspectFinder
   {
      public List<NetAspectDefinition> Find(IEnumerable<Type> types_P)
      {
         return types_P.
            Select(t => new NetAspectDefinition(t)).
            Where(IsValid)
            .ToList();
      }

       private bool IsValid(NetAspectDefinition aspect)
      {
         return aspect.Type.FieldExists("NetAspectAttribute");
      }
   }
}
