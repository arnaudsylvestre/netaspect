using System.Reflection;
using FluentAspect.Weaver.Tests.Core;
using FluentAspect.Weaver.Tests.unit;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Method.Parameters.Before.Instance
{
   public class InstanceParameterBeforeTest : SimpleAcceptanceTest
   {
      protected override void ConfigureAssembly(AssemblyDefinitionDefiner assembly)
      {
         var aspect = CreateAspect(assembly);
         Weave(assembly, aspect);
      }

      private static void Weave(AssemblyDefinitionDefiner assembly, MethodWeavingAspectDefiner aspect)
      {
         var method = assembly.WithType("MyClassToWeave").WithMethod("MyMethodToWeave");
         method.AddAspect(aspect);
      }

      private static MethodWeavingAspectDefiner CreateAspect(AssemblyDefinitionDefiner assembly)
      {
         var aspect = assembly.WithMethodWeavingAspect("MyAspectAttribute");
         aspect.AddBefore()
               .WithParameter<object>("instance");
         return aspect;
      }

      protected override void EnsureAssembly(Assembly assembly_P)
      {
         assembly_P.GetType()
      }
   }
}