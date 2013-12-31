using System;
using FluentAspect.Sample.Attributes;
using FluentAspect.Sample.Dep;

namespace FluentAspect.Sample
{
    public class MyClassToWeave
    {

        public void WeavedThroughAssembly()
        {
        }

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


        [CheckOnCallNetAspectAttribute]
        public string WeavedOnCall(string parameter)
        {
           return "Hello";
        }

        [CheckOnCallAfterNetAspectAttribute]
        public string WeavedOnCallAfter(string parameter)
        {
           return "Hello";
        }

        [CheckLineNumberOnCallAfterNetAspectAttribute]
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

        [CheckWithParameterNameInterceptorNetAspectAttribute]
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

        [CheckReturnSimpleTypeNetAspect]
        public int CheckWithReturnSimpleType()
        {
            return 0;
        }

        [CheckThrowInterceptorNetAspect]
        public string CheckThrowWithReturn()
        {
            throw new NotImplementedException();
        }

       public void CallWeavedOnCallAfterWithParameters(string callerMethodParameter)
       {
          WeavedOnCallAfterWithParameters();
       }

       [CheckParametersCallerOnCallAfterNetAspectAttribute]
       public void WeavedOnCallAfterWithParameters()
       {
          
       }
    }

   public class CheckParametersCallerOnCallAfterNetAspectAttribute : Attribute
   {

      public static void AfterCall(string callerMethodParameterCaller)
      {
         throw new Exception(callerMethodParameterCaller);
      }
   }

   public class CheckOnCallNetAspectAttribute : Attribute
    {
       public static void BeforeCall()
       {
          throw new NotSupportedException();
       }

    }

    public class CheckOnCallAfterNetAspectAttribute : Attribute
    {
       public static void AfterCall()
       {
          throw new NotSupportedException();
       }

    }

    public class CheckLineNumberOnCallAfterNetAspectAttribute : Attribute
    {
       public static void AfterCall(int lineNumber, int columnNumber, string filename)
       {
          throw new Exception(lineNumber.ToString() + " : " + columnNumber.ToString() + " : " + filename);
       }

    }

    public class CheckReturnSimpleTypeNetAspectAttribute : Attribute
    {
        public void After(ref int result)
        {
            result = 5;
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