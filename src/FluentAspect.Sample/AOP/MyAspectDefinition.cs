using FluentAspect.Core;

namespace FluentAspect.Sample
{
   public class MyAspectDefinition : FluentAspectDefinition
   {
      public override void Setup()
      {
         WeaveMethodWhichMatches<CheckWithReturnInterceptor>(m => m.Name == "CheckWithReturn");
         WeaveMethodWhichMatches<CheckWithParametersInterceptor>(m => m.Name == "CheckWithParameters");
      }
   }
}