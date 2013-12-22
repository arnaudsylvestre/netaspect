using System;
using FluentAspect.Core.Attributes;
using FluentAspect.Sample.Attributes;

namespace FluentAspect.Sample
{

    public class MyClassToWeave
    {

        [MethodInterceptor(typeof(CheckWithReturnInterceptor))]
       public string CheckWithReturn()
       {
          return "NotWeaved";
       }
        [MethodInterceptor(typeof(CheckWithParametersInterceptor))]
        
       public string CheckWithParameters(string aspectWillReturnThis)
       {
          return "NotWeaved";
       }


        [MethodInterceptor(typeof(CheckWithVoidInterceptor))]
       public void CheckWithVoid()
       {

       }

       [MethodInterceptor(typeof(CheckWithGenericsInterceptor))]
       public string CheckWithGenerics<T>(T arg)
       {
           return arg.ToString() + "<>" + typeof(T).FullName;
       }


       [MethodInterceptor(typeof(CheckWithGenericsInterceptor))]
       public string CheckWithGenericsClass<T>(T arg)
           where T : class 
       {
           return arg.ToString() + "<>" + typeof(T).FullName;
       }

       [MethodInterceptor(typeof(CheckThrowInterceptor))]
        public void CheckThrow()
        {
            throw new NotImplementedException();
        }


       [MethodInterceptor(typeof(CheckBeforeInterceptor))]
        public string CheckBefore(BeforeParameter parameter)
        {
            return parameter.Value;
        }

        [MethodInterceptor(typeof(CheckWithParameterNameInterceptor))]
        public string CheckWithParameterName(int first, int second)
        {
            return string.Format("{0} : {1}", first, second);
        }

        [MethodInterceptor(typeof(CheckBeforeInterceptor))]
        public static string CheckStatic(BeforeParameter parameter)
        {
            return parameter.Value;
        }



        [MethodInterceptor(typeof(CheckNotRenameInAssemblyInterceptor))]
        public string CheckNotRenameInAssembly()
        {
            return CheckWithReturn();
        }



        [MethodInterceptor(typeof(MockInterceptor))]
        public string CheckMockException()
        {
            throw new NotImplementedException();
        }
        [MethodInterceptor(typeof(MockInterceptor))]
        public string CheckMock(string parameter)
        {
            return "return";
        }

        [CheckMultiInterceptor]
        [CheckMultiInterceptor]
        public string CheckMulti(int i)
        {
            return i.ToString();
        }
    }

    public class CheckWithParameterNameInterceptor
    {
        public void Before(int first, ref int second)
        {
            second = first + 1;
        }
    }
}