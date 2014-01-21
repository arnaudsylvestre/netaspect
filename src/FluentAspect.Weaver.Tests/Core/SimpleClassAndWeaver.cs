﻿namespace FluentAspect.Weaver.Tests.Core
{
    public class SimpleClassAndWeaver
    {
        public TypeDefinitionDefiner ClassToWeave { get; set; }

        public MethodWeavingAspectDefiner Aspect { get; set; }

        public MethodDefinitionDefiner MethodToWeave { get; set; }

        public MethodDefinitionDefiner BeforeInterceptor
        {
            get { return Aspect.AddBefore(); }
        }

        public MethodDefinitionDefiner AfterInterceptor
        {
            get { return Aspect.AddAfter(); }
        }

        public MethodDefinitionDefiner OnExceptionInterceptor
        {
            get { return Aspect.AddOnException(); }
        }

        public MethodDefinitionDefiner OnFinallyInterceptor
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}