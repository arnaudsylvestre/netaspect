﻿using System;
using FluentAspect.Sample.MethodWeaving.Parameters.Parameters.Usual;

namespace FluentAspect.Sample.MethodWeaving.Problems.Errors
{
    public class ToCheckParametersReferencedInBeforeAspectAttribute : Attribute
    {
        public string NetAspectAttributeKind = "MethodWeaving";

        public void Before(ref object[] parameters)
        {
            throw new ParametersException(parameters);
        }
    }
}