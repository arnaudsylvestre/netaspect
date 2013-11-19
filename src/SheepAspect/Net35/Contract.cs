namespace System.Diagnostics.Contracts
{
   public class Contract
   {
      public static void Requires(bool flag_P)
      {
         if (!flag_P)
            throw new Exception();
      }
   }
}