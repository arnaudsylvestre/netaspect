using System.Reflection;

namespace FluentAspect.Weaver.Tests.Core
{
   public interface IAcceptanceTestBuilder<TSample, TActual>
   {
      TSample CreateSample(AssemblyDefinitionDefiner definer);
      TActual CreateActual(Assembly assembly_P);
   }
}