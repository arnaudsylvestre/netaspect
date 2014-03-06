﻿using System;
using FluentAspect.Sample.AOP;
using FluentAspect.Sample.Dep;

namespace FluentAspect.Sample
{
    public static class MyClassToWeaveFactory
    {
        public static MyClassToWeave Create()
        {
            return new MyClassToWeave();
        }
    }

    public class MyClassToWeave
    {
        [CheckOnCallAfter]
        public MyClassToWeave()
        {
        }

        public void WeavedThroughAssembly()
        {
        }

        [CheckWithReturn]
        public string CheckWithReturn()
        {
            return "NotWeaved";
        }

        [CheckWithParametersInterceptor]
        public string CheckWithParameters(string aspectWillReturnThis)
        {
            return "NotWeaved";
        }

        [CheckWithParametersReferenced]
        public string CheckWithParametersReferenced(string aspectWillReturnThis)
        {
            return "NotWeaved";
        }


        [CheckWithVoidInterceptor]
        public void CheckWithVoid()
        {
        }

        [CheckWithGenericsInterceptor]
        public string CheckWithGenerics<T>(T arg)
        {
            return arg + "<>" + typeof (T).FullName;
        }


        [CheckWithGenericsInterceptor]
        public string CheckWithGenericsClass<T>(T arg)
            where T : class
        {
            return arg + "<>" + typeof (T).FullName;
        }

        [CheckThrowInterceptor]
        public void CheckThrow()
        {
            throw new NotImplementedException();
        }


        [CheckOnCallBefore]
        public string WeavedOnCall(string parameter)
        {
            return "Hello";
        }

        [CheckOnCallAfter]
        public string WeavedOnCallAfter(string parameter)
        {
            return "Hello";
        }

        [CheckLineNumberOnCallAfter]
        public void WeavedOnCallAfter()
        {
        }

        public string CallWeavedOnCall(string parameter)
        {
            return WeavedOnCall(parameter);
        }

        public string CallWeavedOnCallAfter(string parameter)
        {
            return WeavedOnCallAfter(parameter);
        }

        public void CallWeavedOnCallAfter()
        {
            WeavedOnCallAfter();
        }


        [CheckBeforeAspect]
        public string CheckBefore(BeforeParameter parameter)
        {
            return parameter.Value;
        }

        [CheckWithParameterNameInterceptor]
        public string CheckWithParameterName(int first, int second)
        {
            return string.Format("{0} : {1}", first, second);
        }

        [CheckBeforeAspect]
        public static string CheckStatic(BeforeParameter parameter)
        {
            return parameter.Value;
        }

        public void CheckDependency(string o)
        {
            new DepClassToWeave().EnsureNotNull(o);
        }

        [CheckNotRenameInAssembly]
        public string CheckNotRenameInAssembly()
        {
            return CheckWithReturn();
        }


        [MockInterceptor]
        public string CheckMockException()
        {
            throw new NotImplementedException();
        }

        [MockInterceptor]
        public string CheckMock(string parameter)
        {
            return "return";
        }

        [CheckMulti]
        [CheckMulti]
        public string CheckMulti(int i)
        {
            return i.ToString();
        }

        [CheckReturnSimpleType]
        public int CheckWithReturnSimpleType()
        {
            return 0;
        }

        [CheckThrowInterceptor]
        public string CheckThrowWithReturn()
        {
            throw new NotImplementedException();
        }

        public void CallWeavedOnCallAfterWithParameters(string callerMethodParameter)
        {
            WeavedOnCallAfterWithParameters();
        }

        [CheckParametersCallerOnCallAfter]
        public void WeavedOnCallAfterWithParameters()
        {
        }

        public void CallCheckCaller()
        {
            CheckCaller();
        }

        [CheckCaller]
        public void CheckCaller()
        {
        }

        public void CallWeavedOnCallAfterWithParameterCalled()
        {
            WeavedOnCallAfterWithParameters("Parameter", "intercepted");
        }


        [CheckParametersCalledOnCallAfter]
        public void WeavedOnCallAfterWithParameters(string parameter1, string parameter2)
        {
        }

        [CheckAfterCallParameterTypeOnCallAfter]
        public void AfterCallParametersWithWrongTypeCalled(string parameter1, string parameter2)
        {
        }

        public void AfterCallParametersWithWrongType(string parameter1, string parameter2)
        {
            AfterCallParametersWithWrongTypeCalled(parameter1, parameter2);
        }
    }

    public class CheckAfterCallParameterTypeOnCallAfterAttribute : Attribute
    {
        public string NetAspectAttributeKind = "CallWeaving";

        public static void AfterCall(string lineNumber, string columnNumber, int fileName, int filePath)
        {
            throw new NotSupportedException();
        }
    }

    public class CheckCallerAttribute : Attribute
    {
        public string NetAspectAttributeKind = "CallWeaving";

        public static void AfterCall(object caller)
        {
            throw new Exception(caller.GetType() == typeof (MyClassToWeave) ? "OK" : "KO");
        }
    }

    public class CheckParametersCallerOnCallAfterAttribute : Attribute
    {
        public string NetAspectAttributeKind = "CallWeaving";

        public static void AfterCall(string callerMethodParameterCaller)
        {
            throw new Exception(callerMethodParameterCaller);
        }
    }

    public class CheckParametersCalledOnCallAfter : Attribute
    {
        public string NetAspectAttributeKind = "CallWeaving";

        public static void AfterCall(string parameter1Called, string parameter2Called)
        {
            throw new Exception(parameter1Called + " " + parameter2Called);
        }
    }

    public class CheckOnCallBeforeAttribute : Attribute
    {
        public string NetAspectAttributeKind = "CallWeaving";

        public static void BeforeCall()
        {
            throw new NotSupportedException();
        }
    }

    public class CheckOnCallAfterAttribute : Attribute
    {
        public string NetAspectAttributeKind = "CallWeaving";

        public static void AfterCall()
        {
            throw new NotSupportedException();
        }
    }

    public class CheckLineNumberOnCallAfterAttribute : Attribute
    {
        public string NetAspectAttributeKind = "CallWeaving";

        public static void AfterCall(int lineNumber, int columnNumber, string filename)
        {
            throw new Exception(lineNumber.ToString() + " : " + columnNumber.ToString() + " : " + filename);
        }
    }

    public class CheckReturnSimpleTypeAttribute : Attribute
    {
        public string NetAspectAttributeKind = "MethodWeaving";

        public void After(ref int result)
        {
            result = 5;
        }
    }

    public class CheckWithParameterNameInterceptorAttribute : Attribute
    {
        public string NetAspectAttributeKind = "MethodWeaving";

        public void Before(int first, ref int second)
        {
            second = first + 1;
        }
    }
}