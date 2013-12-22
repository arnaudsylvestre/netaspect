using System;
using FluentAspect.Sample.Attributes;

namespace FluentAspect.Sample
{
    public class MyClassToWeave
    {

        [CheckWithReturnInterceptorNetAspect]
       public string CheckWithReturn()
       {
          return "NotWeaved";
       }
        [CheckWithParametersInterceptorNetAspect]
        
       public string CheckWithParameters(string aspectWillReturnThis)
       {
          return "NotWeaved";
       }


        [CheckWithVoidInterceptorNetAspect]
       public void CheckWithVoid()
       {

       }

        [CheckWithGenericsInterceptorNetAspect]
       public string CheckWithGenerics<T>(T arg)
       {
           return arg.ToString() + "<>" + typeof(T).FullName;
       }


        [CheckWithGenericsInterceptorNetAspect]
       public string CheckWithGenericsClass<T>(T arg)
           where T : class 
       {
           return arg.ToString() + "<>" + typeof(T).FullName;
       }

        [CheckThrowInterceptorNetAspect]
        public void CheckThrow()
        {
            throw new NotImplementedException();
        }


        [CheckBeforeNetAspect]
        public string CheckBefore(BeforeParameter parameter)
        {
            return parameter.Value;
        }

        [CheckWithParameterNameInterceptorNetAspectAttribute]
        public string CheckWithParameterName(int first, int second)
        {
            return string.Format("{0} : {1}", first, second);
        }

        [CheckBeforeNetAspect]
        public static string CheckStatic(BeforeParameter parameter)
        {
            return parameter.Value;
        }



        [CheckNotRenameInAssemblyNetAspectAttribute]
        public string CheckNotRenameInAssembly()
        {
            return CheckWithReturn();
        }



        [MockInterceptorNetAspect]
        public string CheckMockException()
        {
            throw new NotImplementedException();
        }
        [MockInterceptorNetAspect]
        public string CheckMock(string parameter)
        {
            return "return";
        }

        [CheckMultiNetAspect]
        [CheckMultiNetAspect]
        public string CheckMulti(int i)
        {
            return i.ToString();
        }
    }

    public class CheckWithParameterNameInterceptorNetAspectAttribute : Attribute
    {
        public void Before(int first, ref int second)
        {
            second = first + 1;
        }
    }
}