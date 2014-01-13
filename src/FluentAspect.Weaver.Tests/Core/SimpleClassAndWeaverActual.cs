using System.Reflection;
using FluentAspect.Weaver.Tests.Core;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Method.Parameters.Before
{
   public class SimpleClassAndWeaverActual
   {
      public NetAspectAttribute Aspect { get; set; }
      private string classNameToWeave;
      private string methodNameToWeave;
      private Assembly assembly;

      public SimpleClassAndWeaverActual(Assembly assembly_P, string classNameToWeave_P, string methodNameToWeave_P)
      {
         assembly = assembly_P;
         classNameToWeave = classNameToWeave_P;
         methodNameToWeave = methodNameToWeave_P;
      }

      public object CreateObjectFromClassToWeaveType(params object[] parameters)
      {
         return assembly.CreateObject(classNameToWeave, parameters);
      }

      public void CallWeavedMethod(object o_P, params object[] parameters)
      {
         o_P.CallMethod(methodNameToWeave, parameters);
      }
   }
}