﻿using System;
using FluentAspect.Core.Core;
using FluentAspect.Core.Expressions;

namespace FluentAspect.Sample
{
    public class CheckNotRenameInAssemblyInterceptor : IInterceptor
    {
        public void Before(MethodCall call_P)
        {
        }

        public void After(MethodCall call_P, MethodCallResult result_P)
        {
        }

        public void OnException(MethodCall callP_P, ExceptionResult e)
        {
        }
    }
}