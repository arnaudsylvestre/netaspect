using System;
using System.Collections.Generic;

namespace FluentAspect.Weaver.Core.Configuration
{
   public class MethodMatch
   {
      public Func<IMethod, bool> Matcher { get; set; }

      public List<NetAspectAttribute> Interceptors { get; set; }
   }
}