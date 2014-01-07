using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Helpers;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.Configuration.Attributes.Helpers
{
    public class MethodBaseAttributeConfigurationReaderHelper
    {
        public static void Fill(IEnumerable<MethodBase> methods, List<MethodMatch> matches, Func<Assembly, AssemblyDefinition> asmDefinitionProvider)
        {
            foreach (MethodBase matchingMethod in methods)
            {
                MethodBase info = matchingMethod;
                matches.Add(new MethodMatch
                    {
                        AssembliesToScan = new List<AssemblyDefinition>() {
                    asmDefinitionProvider(info.DeclaringType.Assembly)
                    },
                        MethodWeavingInterceptors = matchingMethod.GetMethodWeavingAspectAttributes().ToList(),
                        Matcher =
                            m =>
                            m.AreEqual(info)
                    });
            }
        }

        
    }
}