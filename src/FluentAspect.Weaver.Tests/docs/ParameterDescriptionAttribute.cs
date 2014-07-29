using System;

namespace NetAspect.Weaver.Tests.docs.MethodPossibilities.Before
{
   [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
   public class ParameterDescriptionAttribute : Attribute
   {
      private readonly string _instance;
      private readonly string _theInstance;


      public ParameterDescriptionAttribute(string instance_P, string theInstance_P)
      {
         _instance = instance_P;
         _theInstance = theInstance_P;
      }
   }
}