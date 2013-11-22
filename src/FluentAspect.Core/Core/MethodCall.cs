namespace FluentAspect.Core.Expressions
{
   public class MethodCall
   {
      /// <summary>
      /// Initialise une nouvelle instance de la classe <see cref="T:System.Object"/>.
      /// </summary>
      public MethodCall(object this_P, string methodName_P, object[] parameters_P)
      {
         This = this_P;
         MethodName = methodName_P;
         Parameters = parameters_P;
      }

      public object This { get; private set; }
      public string MethodName { get; private set; }
      public object[] Parameters { get; private set; }
   }
}