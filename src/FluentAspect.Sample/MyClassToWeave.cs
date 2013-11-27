using System;

namespace FluentAspect.Sample
{
    public class BeforeParameter
    {
        public string Value { get; set; }
    }

    public class MyClassToWeave
    {
       public string CheckWithReturn()
       {
          return "NotWeaved";
       }

       public string CheckWithParameters(string aspectWillReturnThis)
       {
          return "NotWeaved";
       }

       public void CheckWithVoid()
       {

       }

        public string CheckWithGenerics<T>(T arg)
        {
           return arg.ToString() + "<>" + typeof(T).FullName;
        }

        public void CheckThrow()
        {
            throw new NotImplementedException();
        }


        public string CheckBefore(BeforeParameter parameter)
        {
            return parameter.Value;
        }

       public string CheckNotRenameInAssembly()
       {
           return CheckWithReturn();
       }
    }
}