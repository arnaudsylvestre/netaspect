using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Core;

namespace FluentAspect.Weaver.Core
{
   public class WeavingConfiguration
   {
      public WeavingConfiguration()
      {
         Methods = new List<MethodMatch>();
          Constructors = new List<ConstructorMatch>();
      }

      public List<MethodMatch> Methods { get; private set; }

       public List<ConstructorMatch> Constructors { get; private set; }
   }
}