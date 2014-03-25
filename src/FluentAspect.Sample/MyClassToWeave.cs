using System;
using FluentAspect.Sample.AOP;
using FluentAspect.Sample.Dep;

namespace FluentAspect.Sample
{
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
}