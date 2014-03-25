﻿using System;

namespace FluentAspect.Sample.MethodWeaving.Problems.Errors
{
    public class ToCheckInstanceWithWrongTypeInAfterAspectAttribute : Attribute
    {
        public string NetAspectAttributeKind = "MethodWeaving";

        public void After(int instance)
        {
        }
    }
}