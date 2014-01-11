using System;
using FluentAspect.Weaver.Tests.Core;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Method.Parameters.Before
{
    public class OnExceptionInstanceParameterTest : InstanceParameterTest
    {
        public OnExceptionInstanceParameterTest()
            : base(a => a.AddOnException())
        {
        }
    }
}