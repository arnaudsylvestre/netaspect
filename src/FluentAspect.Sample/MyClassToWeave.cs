using System;

namespace FluentAspect.Sample
{
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
            return arg.ToString();
        }

        public void CheckThrow()
        {
            throw new NotImplementedException();
        }


        public string CheckBefore(string content)
        {
            return content;
        }

       public string CheckNotRenameInAssembly()
       {
           return CheckWithReturn();
       }
    }
}