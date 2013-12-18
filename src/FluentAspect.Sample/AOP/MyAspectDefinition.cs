﻿using FluentAspect.Core;

namespace FluentAspect.Sample
{
   public class MyAspectDefinition : FluentAspectDefinition
   {
      public override void Setup()
       {
           WeaveMethodWhichMatches<CheckBeforeInterceptor>(m => m.Name == "CheckBefore");
         WeaveMethodWhichMatches<CheckNotRenameInAssemblyInterceptor>(m => m.Name == "CheckNotRenameInAssembly");
         WeaveMethodWhichMatches<CheckThrowInterceptor>(m => m.Name == "CheckThrow");
         WeaveMethodWhichMatches<CheckWithGenericsInterceptor>(m => m.Name == "CheckWithGenerics");
         WeaveMethodWhichMatches<CheckWithGenericsInterceptor>(m => m.Name == "CheckWithGenericsClass");
         WeaveMethodWhichMatches<CheckWithParametersInterceptor>(m => m.Name == "CheckWithParameters");
         WeaveMethodWhichMatches<CheckWithReturnInterceptor>(m => m.Name == "CheckWithReturn");
         WeaveMethodWhichMatches<CheckWithVoidInterceptor>(m => m.Name == "CheckWithVoid");
         WeaveMethodWhichMatches<CheckBeforeInterceptor>(m => m.Name == "CheckStatic");
         WeaveMethodWhichMatches<MockInterceptor>(m => m.Name == "CheckMockException");
         WeaveMethodWhichMatches<MockInterceptor>(m => m.Name == "CheckMock");
      }
   }
}