﻿using System;

namespace FluentAspect.Sample.MethodWeaving.Parameters.Parameters.Usual
{
    public class ToCheckParametersOnException
    {
        [ToCheckParametersOnExceptionAspect]
        public void Check(string parameter1, int parameter2)
        {
            throw new Exception();
        }
    }

    public class ToCheckParametersOnExceptionAspectAttribute : Attribute
    {

        public string NetAspectAttributeKind = "MethodWeaving";

        public void OnException(object[] parameters)
        {
            throw new ParametersException(parameters);
        }
    }
}