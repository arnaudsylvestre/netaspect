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
    }
}