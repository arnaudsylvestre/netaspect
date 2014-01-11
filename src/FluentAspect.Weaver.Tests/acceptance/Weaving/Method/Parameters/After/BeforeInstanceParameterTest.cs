using System;
using FluentAspect.Weaver.Tests.Core;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Method.Parameters.Before
{
    public class AfterInstanceParameterTest : InstanceParameterTest
    {
        public AfterInstanceParameterTest()
            : base(a => a.AddAfter())
        {
        }
    }
}