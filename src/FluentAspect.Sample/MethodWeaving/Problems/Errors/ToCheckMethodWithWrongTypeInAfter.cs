﻿namespace FluentAspect.Sample.MethodWeaving.Problems.Errors
{
    public class ToCheckMethodWithWrongTypeInAfter
    {
        [ToCheckMethodWithWrongTypeInAfterAspect]
        public void Check(string parameter1, int parameter2)
        {
        }
    }
}