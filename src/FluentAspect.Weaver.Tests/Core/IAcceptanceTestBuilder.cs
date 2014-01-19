using System.Reflection;
using FluentAspect.Weaver.Tests.Core;

namespace FluentAspect.Weaver.Tests.acceptance.Weaving.Method.Parameters.Before
{
   public interface IAcceptanceTestBuilder<TSample, TActual>
   {
      TSample CreateSample(AssemblyDefinitionDefiner definer);
      TActual CreateActual(Assembly assembly_P);
   }
}