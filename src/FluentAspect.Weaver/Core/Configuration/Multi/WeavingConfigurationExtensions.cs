namespace FluentAspect.Weaver.Core.Configuration.Multi
{
   public static class WeavingConfigurationExtensions
   {
       public static void MergeWith(this WeavingConfiguration original, WeavingConfiguration toAdd)
       {
           original.Methods.AddRange(toAdd.Methods);
           original.Constructors.AddRange(toAdd.Constructors);
       }
   }
}