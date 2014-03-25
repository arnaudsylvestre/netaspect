﻿using System;

namespace FluentAspect.Sample
{
    public class CheckReturnSimpleTypeAttribute : Attribute
    {
        public string NetAspectAttributeKind = "MethodWeaving";

        public void After(ref int result)
        {
            result = 5;
        }
    }
}