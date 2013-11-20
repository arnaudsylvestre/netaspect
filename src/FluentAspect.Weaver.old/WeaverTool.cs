using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAspect.Core;
using Mono.Cecil;
using SheepAspect.AroundAdvising;
using SheepAspect.Compile;
using SheepAspect.Core;

namespace FluentAspect.Weaver
{
   public class WeaverTool
   {
      public void Weave(CompilerSettings settings)
      {
         AspectCompiler compiler = new AspectCompiler(settings);
         List<AspectDefinition> definitions_L = new List<AspectDefinition>();

         var definition = Assembly.LoadFrom(settings.AspectAssemblyFiles.ToList()[0]);
         
         foreach (var typeDefinition_L in definition.GetTypes())
         {
            if (typeDefinition_L.BaseType == null)
               continue;
            if (typeDefinition_L.BaseType.FullName == typeof(FluentAspectDefinition).FullName)
            {
               FluentAspectDefinition instance_L = (FluentAspectDefinition) Activator.CreateInstance(typeDefinition_L);
               instance_L.Setup();
               
               AspectDefinition def = new AspectDefinition(typeDefinition_L);

               var methodMatches_L = instance_L.methodMatches;
               foreach (var methodMatch_L in methodMatches_L)
               {
                  var pointcut_L = def.AddPointcut<FluentMethodPointcut>("First");
                  pointcut_L.SetMatcher(methodMatch_L.Matcher);
                  var methodInfo_L = typeDefinition_L.GetMethod(methodMatch_L.AdviceName);
                  def.Advise(new FluentAroundAdvice(new List<IPointcut>() { pointcut_L }, methodInfo_L));
               }

               definitions_L.Add(def);
            }
         }

         compiler.Weave(definitions_L);
      }
   }
}