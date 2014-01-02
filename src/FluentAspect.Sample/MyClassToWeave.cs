using System;
using FluentAspect.Sample.AOP;
using FluentAspect.Sample.Dep;

namespace FluentAspect.Sample
{
    public class MyClassToWeave
    {

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
           return arg.ToString() + "<>" + typeof(T).FullName;
       }


        [CheckWithGenericsInterceptor]
       public string CheckWithGenericsClass<T>(T arg)
           where T : class 
       {
           return arg.ToString() + "<>" + typeof(T).FullName;
       }

        [CheckThrowInterceptor]
        public void CheckThrow()
        {
            throw new NotImplementedException();
        }


        [CheckOnCall]
        public string WeavedOnCall(string parameter)
        {
           return "Hello";
        }

        [CheckOnCallAfterAttribute]
        public string WeavedOnCallAfter(string parameter)
        {
           return "Hello";
        }

        [CheckLineNumberOnCallAfterAttribute]
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

        [CheckWithParameterNameInterceptorAttribute]
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
    }

   public class CheckCallerAttribute : Attribute
   {
      string NetAspectAttributeKind = "CallWeaving";

      public static void AfterCall(object caller)
      {
         throw new Exception(caller.GetType() == typeof(MyClassToWeave) ? "OK" : "KO");
      }
   }

   public class CheckParametersCallerOnCallAfterAttribute : Attribute
   {
      string NetAspectAttributeKind = "CallWeaving";

      public static void AfterCall(string callerMethodParameterCaller)
      {
         throw new Exception(callerMethodParameterCaller);
      }
   }

   public class CheckOnCallAttribute : Attribute
    {
      string NetAspectAttributeKind = "CallWeaving";

       public static void BeforeCall()
       {
          throw new NotSupportedException();
       }

    }

    public class CheckOnCallAfterAttribute : Attribute
    {

       string NetAspectAttributeKind = "CallWeaving";

       public static void AfterCall()
       {
          throw new NotSupportedException();
       }

    }

    public class CheckLineNumberOnCallAfterAttribute : Attribute
    {
       string NetAspectAttributeKind = "CallWeaving";

       public static void AfterCall(int lineNumber, int columnNumber, string filename)
       {
          throw new Exception(lineNumber.ToString() + " : " + columnNumber.ToString() + " : " + filename);
       }

    }

    public class CheckReturnSimpleTypeAttribute : Attribute
    {
       string NetAspectAttributeKind = "MethodWeaving";

        public void After(ref int result)
        {
            result = 5;
        }
    }

    public class CheckWithParameterNameInterceptorAttribute : Attribute
    {
       string NetAspectAttributeKind = "MethodWeaving";
        public void Before(int first, ref int second)
        {
            second = first + 1;
        }
    }
}