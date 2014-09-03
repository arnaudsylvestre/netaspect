namespace NetAspect.Sample.Dep
{
   public class DepClassWhichCallField
   {
      public string CallField(string fieldValue)
      {
         var c = new DepClassWithField
         {
            Field = fieldValue
         };
         return c.Field;
      }
   }

   public class DepClassWhichCallProperty
   {
      public string CallProperty(string fieldValue)
      {
         var c = new DepClassWithProperty
         {
            Property = fieldValue
         };
         return c.Property;
      }
   }
}
