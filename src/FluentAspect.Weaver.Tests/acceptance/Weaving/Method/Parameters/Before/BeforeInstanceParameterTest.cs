using System;
using FluentAspect.Weaver.Tests.Core;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Method.Parameters.Before
{
    public class BeforeInstanceParameterTest : InstanceParameterTest
    {
        public BeforeInstanceParameterTest() : base(a => a.AddBefore())
        {
        }
    }
}